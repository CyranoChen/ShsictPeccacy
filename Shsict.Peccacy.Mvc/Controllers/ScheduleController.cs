using System.Web.Mvc;
using Shsict.Peccacy.Mvc.Models;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Scheduler;

namespace Shsict.Peccacy.Mvc.Controllers
{
    public class ScheduleController : Controller
    {
        // GET: Scheduler
        public ActionResult Index()
        {
            var model = new ConsoleModels.ScheduleListDto();

            using (IRepository repo = new Repository())
            {
                model.Schedules = repo.All<Schedule>();
            }

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

                        return Json("Success");
                    }
                }
            }

            return Json("Failed");
        }
    }
}