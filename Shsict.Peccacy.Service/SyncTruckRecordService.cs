using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Extension;
using Shsict.Peccacy.Service.Logger;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Service
{
    public static class SyncTruckRecordService
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

        // 调用指定摄像头数据源到目标库
        public static void SyncCameraSource(CameraSource cam)
        {
            IUserLog log = new UserLog();

            try
            {
                using (IRepository repo = new Repository())
                {
                    // 获取需要同步的集卡违章数据，已按抓拍时间倒序排列
                    var list = GetTruckRecordsByCamSource(cam);

                    if (list.Count > 0)
                    {
                        // 同步到oracle库中，返回成功记录数
                        var countInsert = list.AsEnumerable().Insert();

                        // 获取最后记录的时间戳
                        cam.LastSyncTime = list[0].PicTime;

                        repo.Save(cam);

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

        private static List<TruckCamRecord> GetTruckRecordsByCamSource(CameraSource cam)
        {
            IUserLog log = new UserLog();

            var list = new List<TruckCamRecord>();

            var sql = $"SELECT * FROM {cam.ViewName} WHERE PICTIME > @pictime ORDER BY PICTIME DESC";

            var ds = AccessHelper.ExecuteDataset(cam.ConnString, CommandType.Text, sql,
                new OleDbParameter("@pictime", cam.LastSyncTime));

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
    }
}