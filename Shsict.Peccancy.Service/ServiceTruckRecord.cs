using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Shsict.Peccancy.Service.DbHelper;
using Shsict.Peccancy.Service.Extension;
using Shsict.Peccancy.Service.Logger;
using Shsict.Peccancy.Service.Model;

namespace Shsict.Peccancy.Service
{
    public static class ServiceTruckRecord
    {
        // 主程序：调用一次同步全部摄像头数据源到目标库
        public static void SyncAllCameraSources()
        {
            // 获取已配置的全部摄像头数据源实体(缓存中取出)
            var cams = CameraSource.Cache.CameraSourceList.FindAll(x => x.IsSync);

            if (cams.Count > 0)
            {
                foreach (var cam in cams)
                {
                    SyncCameraSource(cam);
                }
            }
        }

        // 主程序：调用一次清理全部摄像头数据源
        public static void ClearAllCameraSources(int timeSpanLimit = 5)
        {
            // 获取已配置的全部摄像头数据源实体(缓存中取出)
            var cams = CameraSource.Cache.CameraSourceList.FindAll(x => x.IsSync);

            if (cams.Count > 0)
            {
                foreach (var cam in cams)
                {
                    ClearCameraSource(cam, timeSpanLimit);
                }
            }
        }

        // 调用指定摄像头数据源到目标库
        public static void SyncCameraSource(CameraSource cam)
        {
            IUserLog log = new UserLog();

            try
            {
                // 获取需要同步的集卡违章数据，已按抓拍时间升序排列
                var list = GetTruckRecordsByCamSource(cam);

                if (list.Count > 0)
                {
                    using (IRepository repo = new Repository())
                    {
                        // 同步到oracle库中，返回成功记录数
                        var countInsert = list.AsEnumerable().Insert();

                        // 获取最后记录的时间戳
                        cam.LastSyncTime = list.Last().PicTime;

                        repo.Save(cam);

                        CameraSource.Cache.RefreshCache();

                        // 记录成功日志
                        var msg = new
                        {
                            camNo = cam.CamNo,
                            countInsert,
                            lastSyncRecordTime = cam.LastSyncTime.ToString("yyyyMMdd HH:mm:ss")
                        };

                        log.Info(msg.ToJson());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        // 调用指定摄像头数据源的清理冗余数据
        public static void ClearCameraSource(CameraSource cam, int timeSpanLimit = 5)
        {
            IUserLog log = new UserLog();

            try
            {
                // 获取当前摄像头的全部数据记录
                using (IRepository repo = new Repository())
                {
                    var list = repo.Query<TruckCamRecord>(x => x.CamNo.Equals(cam.CamNo)).FindAll(x => !string.IsNullOrEmpty(x.License));

                    if (list.Count > 0)
                    {
                        var query = (from t in list
                                     group t by t.License into g
                                     orderby g.Count() descending
                                     select new { License = g.Key, Nums = g.Count() })
                            .ToList().FindAll(x => x.Nums > 1);

                        if (query.Count > 0)
                        {
                            var countClear = 0;

                            foreach (var instance in query)
                            {
                                var result = list.GetRedundantTruckRecords(instance.License, instance.Nums, timeSpanLimit);

                                //var listDelete = list.FindAll(x => result.Exists(t => t.Equals(x)));

                                if (result.Count > 0)
                                {
                                    foreach (var del in result)
                                    {
                                        countClear += repo.Delete(del);
                                    }
                                }
                            }

                            if (countClear > 0)
                            {
                                // 记录成功日志
                                var msg = new
                                {
                                    camNo = cam.CamNo,
                                    countClear,
                                    //lastSyncRecordTime = cam.LastSyncTime.ToString("yyyyMMdd HH:mm:ss")
                                };

                                log.Info(msg.ToJson());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private static List<TruckCamRecord> GetTruckRecordsByCamSource(CameraSource cam)
        {
            IUserLog log = new UserLog();

            var list = new List<TruckCamRecord>();

            // access 时间字段查询需要在两边加上#，通过oledb parameter传入会报错
            var sql = $"SELECT * FROM {cam.ViewName} WHERE PICTIME > #{cam.LastSyncTime}# ORDER BY PICTIME";

            var ds = AccessHelper.ExecuteDataset(cam.ConnString, CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var dt = ds.Tables[0];

                // 如果记录中的摄像头编号与配置中不同，则记录warn日志，返回默认集合
                if (dt.Rows[0]["CamNo"] != null && !dt.Rows[0]["CamNo"].ToString().Equals(cam.CamNo))
                {
                    log.Warn($"{cam.CamNo}的数据源同步记录中字段CamNo不匹配");
                }
                else
                {
                    // 构造TruckCamRecord, 填充list
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new TruckCamRecord(dr, cam.CamNo));
                    }
                }
            }

            return list;
        }

        private static List<TruckCamRecord> GetRedundantTruckRecords(this List<TruckCamRecord> list, string license, int nums, int timeSpanLimit = 5)
        {
            var query = list.FindAll(x => x.License.Equals(license, StringComparison.OrdinalIgnoreCase));

            var result = new List<TruckCamRecord>();

            // 清理冗余数据
            if (query.Count > 0 && query.Count == nums)
            {
                // 按时间顺序排
                query.Sort((x1, x2) => x1.PicTime.CompareTo(x2.PicTime));

                // 第一个成员作为基准对象
                var r = query[0];

                foreach (var curr in query)
                {
                    // 如果第一条记录，就跳过
                    if (r.Equals(curr)) { continue; }

                    var timeDiff = curr.PicTime - r.PicTime;

                    // 如两条连续记录的时间差小于5秒, timeSpanLimit 外部传入，可以配置，默认5秒
                    if (timeDiff.TotalSeconds < timeSpanLimit)
                    {
                        // 清除速度慢的那条
                        if (r.Speed < curr.Speed)
                        {
                            result.Add(r);
                            r = curr;
                        }
                        else
                        {
                            result.Add(curr);
                        }
                    }
                    else
                    {
                        // 不清除，将当前记录作为基准记录
                        r = curr;
                    }
                }
            }

            return result;
        }
    }
}