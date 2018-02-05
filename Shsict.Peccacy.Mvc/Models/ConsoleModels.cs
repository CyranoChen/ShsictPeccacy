using System.Collections.Generic;
using Shsict.Peccacy.Service.Logger;
using Shsict.Peccacy.Service.Model;
using Shsict.Peccacy.Service.Scheduler;

namespace Shsict.Peccacy.Mvc.Models
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