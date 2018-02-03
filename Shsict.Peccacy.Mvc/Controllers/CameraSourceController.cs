using System.Web.Mvc;
using Shsict.Peccacy.Mvc.Models;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Mvc.Controllers
{
    public class CameraSourceController : Controller
    {
        // GET: CameraSource
        public ActionResult Index()
        {
            var model = new ConsoleModels.CameraSourceListDto();

            using (IRepository repo = new Repository())
            {
                model.CameraSources = repo.All<CameraSource>();
            }

            return View(model);
        }
    }
}