using System.Linq;
using System.Web.Mvc;
using Shsict.Peccacy.Mvc.Models;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Mvc.Controllers
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