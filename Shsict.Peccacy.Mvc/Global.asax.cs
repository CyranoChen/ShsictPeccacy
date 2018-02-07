using System;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Shsict.Peccacy.Mvc.Models;
using Shsict.Peccacy.Service.Logger;
using Shsict.Peccacy.Service.Scheduler;

namespace Shsict.Peccacy.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Timer _eventTimer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (_eventTimer == null && ConfigGlobal.SchedulerActive)
            {
                // 应用启动后1分钟之后，开始按每10秒轮询一次
                _eventTimer = new Timer(SchedulerCallback, null, 60 * 1000, ScheduleManager.TimerSecondsInterval * 1000);
            }
        }

        private void SchedulerCallback(object sender)
        {
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;

            if (declaringType != null)
            {
                var assembly = declaringType.Assembly.GetName().Name;

                ScheduleManager.Execute(assembly);
            }
        }

    }
}
