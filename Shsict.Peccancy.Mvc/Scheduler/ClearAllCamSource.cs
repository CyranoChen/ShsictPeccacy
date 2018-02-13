using System;
using Shsict.Peccancy.Mvc.Models;
using Shsict.Peccancy.Service.Logger;
using Shsict.Peccancy.Service.Scheduler;

namespace Shsict.Peccancy.Mvc.Scheduler
{
    internal class ClearAllCamSource : ISchedule
    {
        private readonly IAppLog _log = new AppLog();

        public void Execute(object state)
        {
            try
            {
                Service.ServiceTruckRecord.ClearAllCameraSources(ConfigGlobal.TimeSpanLimit);

                _log.Info("Scheduler executed: (ClearAllCamSource)");
            }
            catch (Exception ex)
            {
                _log.Warn("Scheduler failed: (ClearAllCamSource)");
                _log.Error(ex);
            }
        }
    }
}