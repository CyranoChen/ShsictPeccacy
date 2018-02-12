using System.Collections.Generic;
using Shsict.Peccancy.Service.Logger;
using Shsict.Peccancy.Service.Model;
using Shsict.Peccancy.Service.Scheduler;

namespace Shsict.Peccancy.Mvc.Models
{
    public class ConsoleModels
    {
        public class ConfigListDto
        {
            public List<Config> Configs { get; set; }
        }

        public class ScheduleListDto
        {
            public List<Schedule> Schedules { get; set; }
        }

        public class LogListDto
        {
            public List<Log> Logs { get; set; }
        }

        public class CameraSourceListDto
        {
            public List<CameraSource> CameraSources { get; set; }
        }

        public class TruckCamRecordListDto
        {
            public List<TruckCamRecord> TruckCamRecords { get; set; }
        }
    }
}