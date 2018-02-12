using System;
using Shsict.Peccancy.Mvc.Models;
using Shsict.Peccancy.Service.Logger;
using Shsict.Peccancy.Service.Model;
using Shsict.Peccancy.Service.Scheduler;

namespace Shsict.Peccancy.Mvc.Scheduler
{
    internal class RefreshCache : ISchedule
    {
        private readonly IAppLog _log = new AppLog();

        public void Execute(object state)
        {
            try
            {
                ConfigGlobal.Refresh();
                Schedule.Cache.RefreshCache();

                CameraSource.Cache.RefreshCache();

                _log.Info("Scheduler executed: (RefreshCache)");
            }
            catch (Exception ex)
            {
                _log.Warn("Scheduler failed: (RefreshCache)");
                _log.Error(ex);
            }
        }
    }
}