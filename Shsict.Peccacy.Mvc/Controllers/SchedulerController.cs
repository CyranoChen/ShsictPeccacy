using System.Web.Mvc;
using Shsict.Peccacy.Mvc.Models;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Scheduler;

namespace Shsict.Peccacy.Mvc.Controllers
{
    public class SchedulerController : Controller
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
    }
}