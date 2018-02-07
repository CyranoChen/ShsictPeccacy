using System;
using Shsict.Peccacy.Service.Logger;
using Shsict.Peccacy.Service.Model;
using Shsict.Peccacy.Service.Scheduler;

namespace Shsict.Peccacy.Mvc.Scheduler
{
    internal class RefreshCache : ISchedule
    {
        private readonly IAppLog _log = new AppLog();

        public void Execute(object state)
        {
            try
            {
                Config.Cache.RefreshCache();
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