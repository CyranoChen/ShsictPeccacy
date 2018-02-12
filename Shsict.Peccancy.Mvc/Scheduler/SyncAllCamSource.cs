using System;
using Shsict.Peccancy.Service.Logger;
using Shsict.Peccancy.Service.Scheduler;

namespace Shsict.Peccancy.Mvc.Scheduler
{
    internal class SyncAllCamSource : ISchedule
    {
        private readonly IAppLog _log = new AppLog();

        public void Execute(object state)
        {
            try
            {
                Service.SyncTruckRecordService.SyncAllCameraSources();

                _log.Info("Scheduler executed: (SyncAllCamSource)");
            }
            catch (Exception ex)
            {
                _log.Warn("Scheduler failed: (SyncAllCamSource)");
                _log.Error(ex);
            }
        }
    }
}