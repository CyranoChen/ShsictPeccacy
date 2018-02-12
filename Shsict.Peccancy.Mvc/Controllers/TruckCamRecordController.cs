using System.Linq;
using System.Web.Mvc;
using Shsict.Peccancy.Mvc.Models;
using Shsict.Peccancy.Service.DbHelper;
using Shsict.Peccancy.Service.Model;

namespace Shsict.Peccancy.Mvc.Controllers
{
    public class TruckCamRecordController : Controller
    {
        // GET: TruckCamRecord
        public ActionResult Index()
        {
            var model = new ConsoleModels.TruckCamRecordListDto();

            using (IRepository repo = new Repository())
            {
                model.TruckCamRecords = repo.All<TruckCamRecord>()
                    .OrderByDescending(x => x.PicTime).ToList();
            }

            return View(model);
        }
    }
}