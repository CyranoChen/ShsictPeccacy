using System.Reflection;
using System.Web.Mvc;
using Shsict.Peccacy.Mvc.Models;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Refresh
        public ActionResult Refresh()
        {
            Service.Model.Config.UpdateAssemblyInfo(Assembly.GetExecutingAssembly());

            ConfigGlobal.Refresh();

            return RedirectToAction("Index", "Home");
        }

        // GET: Console/ConfigManagement
        public ActionResult ConfigManagement()
        {
            var model = new HomeModels.ConfigManagementDto();

            using (IRepository repo = new Repository())
            {
                model.Configs = repo.Query<Config>(x => !x.ConfigKey.Contains("Assembly"));
            }

            return View(model);
        }

        // AJAX JsonResult
        // POST:  Console/Config
        [HttpPost]
        public JsonResult Config(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                using (IRepository repo = new Repository())
                {
                    var config = repo.Single<Config>(x => x.ConfigKey == key);

                    if (config != null)
                    {
                        config.ConfigValue = value;

                        repo.Save(config);

                        ConfigGlobal.Refresh();

                        return Json("Success");
                    }
                }
            }

            return Json("Failed");
        }
    }
}