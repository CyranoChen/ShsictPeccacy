using System.Web.Mvc;
using Shsict.Peccancy.Mvc.Models;
using Shsict.Peccancy.Service.DbHelper;
using Shsict.Peccancy.Service.Scheduler;

namespace Shsict.Peccancy.Mvc.Controllers
{
    public class ScheduleController : Controller
    {
        // GET: Scheduler
        public ActionResult Index()
        {
            var model = new ConsoleModels.ScheduleListDto
            {
                Schedules = Schedule.Cache.ScheduleList
            };

            return View(model);
        }

        // AJAX JsonResult
        // POST:  Schduler/Update
        [HttpPost]
        public JsonResult Update(string key, int seconds, bool isactive)
        {
            if (!string.IsNullOrEmpty(key) && seconds > 0)
            {
                using (IRepository repo = new Repository())
                {
                    var s = repo.Single<Schedule>(x => x.ScheduleKey == key);

                    if (s != null)
                    {
                        s.Seconds = seconds;
                        s.IsActive = isactive;

                        repo.Save(s);

                        Schedule.Cache.RefreshCache();

                        return Json("Success");
                    }
                }
            }

            return Json("Failed");
        }
    }
}