using System;
using System.ComponentModel.DataAnnotations.Schema;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Service.Scheduler
{
    [Table("PECCACY_SCHEDULE")]
    public class Schedule : Entity<int>
    {
        private ISchedule _ischedule;

        /// <summary>
        ///     The current implementation of IScheduler
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public ISchedule IScheduleInstance
        {
            get
            {
                InitISchedule();
                return _ischedule;
            }
        }

        //private static void CreateMap()
        //{
        //    var map = Mapper.CreateMap<IDataReader, Schedule>();

        //    map.ForMember(d => d.ScheduleKey, opt => opt.MapFrom(s => s.GetValue("ScheduleKey").ToString()));

        //    map.ForMember(d => d.Seconds, opt => opt.ResolveUsing(s =>
        //    {
        //        var mins = (int) s.GetValue("Seconds");
        //        if (mins > 0 & mins < ScheduleManager.TimerMinutesInterval)
        //        {
        //            return ScheduleManager.TimerMinutesInterval;
        //        }
        //        return mins;
        //    }));

        //    map.ForMember(d => d.ExecuteTimeInfo, opt => opt.ResolveUsing(s =>
        //    {
        //        var dailyTime = (int) s.GetValue("DailyTime");

        //        if (dailyTime >= 0)
        //        {
        //            return $"Run at {dailyTime/60}:{dailyTime%60}";
        //        }
        //        return $"Run By {s.GetValue("Seconds").ToString()} mins";
        //    }));
        //}

        //public override void Inital()
        //{
        //    if (Seconds > 0 & Seconds < ScheduleManager.TimerMinutesInterval)
        //    {
        //        Seconds = ScheduleManager.TimerMinutesInterval;
        //    }

        //    if (DailyTime >= 0)
        //    {
        //        ExecuteTimeInfo = $"Run at {DailyTime / 60}:{DailyTime % 60}";
        //    }
        //    else
        //    {
        //        ExecuteTimeInfo = $"Run By {Seconds} mins";
        //    }
        //}

        //public Schedule Single(object key)
        //{
        //    var sql = $"SELECT * FROM {Repository.GetTableAttr<Schedule>().Name} WHERE ScheduleKey = @key";

        //    return _conn.QueryFirstOrDefault<Schedule>(sql, new { key });
        //}

        //public bool Any()
        //{
        //    var sql = $"SELECT * FROM {Repository.GetTableAttr<Schedule>().Name} WHERE ScheduleKey = @key";

        //    var result = _conn.Query<int>(sql, new { key = ScheduleKey }).ToList();

        //    return Convert.ToInt32(result[0]) > 0;
        //}

        //public List<Schedule> All()
        //{
        //    var attr = Repository.GetTableAttr<Schedule>();

        //    var sql = $"SELECT * FROM {attr.Name} ORDER BY {attr.Sort}";

        //    var list = _conn.Query<Schedule>(sql).ToList();

        //    //if (list.Count > 0) { list.Each(x => x.Inital()); }
        //    // TODO CREATEMAP

        //    return list;
        //}

        //public void Update(IDbTransaction trans = null)
        //{
        //    Contract.Requires(Any());

        //    var sql =
        //        $@"UPDATE {Repository.GetTableAttr<Schedule>().Name
        //            } SET ScheduleType = @scheduleType, DailyTime = @dailyTime, Seconds = @minutes, 
        //                     LastCompletedTime = @lastCompletedTime, IsSystem = @isSystem, IsActive = @isActive, Remark = @remark 
        //                     WHERE ScheduleKey = @key";

        //    SqlParameter[] para =
        //    {
        //        new SqlParameter("@scheduleType", ScheduleType),
        //        new SqlParameter("@dailyTime", DailyTime),
        //        new SqlParameter("@minutes", Seconds),
        //        new SqlParameter("@lastCompletedTime", LastCompletedTime),
        //        new SqlParameter("@isSystem", IsSystem),
        //        new SqlParameter("@isActive", IsActive),
        //        new SqlParameter("@remark", Remark),
        //        new SqlParameter("@key", ScheduleKey)
        //    };

        //    _conn.Execute(sql, para, trans);
        //}

        /// <summary>
        ///     Private method for loading an instance of ISchedule
        /// </summary>
        private void InitISchedule()
        {
            if (_ischedule == null)
            {
                if (ScheduleType == null)
                {
                    //SchedulerLogs.WriteFailedLog("计划任务没有定义其 type 属性");
                    throw new Exception("计划任务没有定义其 type 属性");
                }

                var type = Type.GetType(ScheduleType);

                if (type == null)
                {
                    //SchedulerLogs.WriteFailedLog(string.Format("计划任务 {0} 无法被正确识别", this.ScheduleType));
                    throw new Exception($"计划任务 {ScheduleType} 没有定义其 type 属性");
                }
                else
                {
                    _ischedule = (ISchedule) Activator.CreateInstance(type);

                    if (_ischedule == null)
                    {
                        //SchedulerLogs.WriteFailedLog(string.Format("计划任务 {0} 未能正确加载", this.ScheduleType));
                        throw new Exception($"计划任务 {ScheduleType} 没有定义其 type 属性");
                    }
                }
            }
        }

        public bool ShouldExecute()
        {
            //If we have a TimeOfDay value, use it and ignore the Seconds interval
            if (DailyTime > -1)
            {
                //Now
                var dtNow = DateTime.Now; //now
                //We are looking for the current day @ 12:00 am
                var dtCompare = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day);
                //Check to see if the LastCompleted date is less than the 12:00 am + TimeOfDay minutes
                return LastCompletedTime < dtCompare.AddMinutes(DailyTime) &&
                       dtCompare.AddMinutes(DailyTime) <= DateTime.Now;
            }
            //Is the LastCompleted date + the Seconds interval less than now?
            return LastCompletedTime.AddSeconds(Seconds) < DateTime.Now;
        }

        #region Members and Properties
        [Column("SCHEDULEKEY")]
        public string ScheduleKey { get; set; }

        /// <summary>
        ///     The Type of class which implements IScheduler
        /// </summary>
        [Column("SCHEDULETYPE")]
        public string ScheduleType { get; set; }

        /// <summary>
        ///     Absolute time in mintues from midnight. Can be used to assure event is only
        ///     executed once per-day and as close to the specified
        ///     time as possible. Example times: 0 = midnight, 27 = 12:27 am, 720 = Noon
        /// </summary>
        [Column("DAILYTIME")]
        public int DailyTime { get; set; }

        /// <summary>
        ///     The scheduled event interval time in minutes. If TimeOfDay has a value >= 0, Seconds will be ignored.
        ///     This values should not be less than the Timer interval.
        /// </summary>
        [Column("SECONDS")]
        public int Seconds { get; set; }

        /// <summary>
        ///     Last Date and Time this scheduler was processed/completed.
        /// </summary>
        [Column("LASTCOMPLETEDTIME")]
        public DateTime LastCompletedTime { get; set; }

        //public DateTime LastCompletedTime
        //{
        //    get { return LastCompletedTime; }
        //    set
        //    {
        //        dateWasSet = true;
        //        LastCompletedTime = value;
        //    }
        //}

        //internal testing variable
        //bool dateWasSet = false;

        [Column("ISSYSTEM")]
        public bool IsSystem { get; set; }

        [Column("ISACTIVE")]
        public bool IsActive { get; set; }

        [Column("REMARK")]
        public string Remark { get; set; }

        [NotMapped]
        public string ExecuteTimeInfo { get; set; }

        #endregion
    }
}