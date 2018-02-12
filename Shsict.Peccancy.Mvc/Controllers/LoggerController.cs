using System;
using System.Linq;
using System.Web.Mvc;
using Shsict.Peccancy.Mvc.Models;
using Shsict.Peccancy.Service.DbHelper;
using Shsict.Peccancy.Service.Logger;

namespace Shsict.Peccancy.Mvc.Controllers
{
    public class LoggerController : Controller
    {
        // GET: Logger
        public ActionResult Index()
        {
            var model = new ConsoleModels.LogListDto();

            using (IRepository repo = new Repository())
            {
                // 非数据库操作日志（1天内）
                var dateLower = DateTime.Now.AddDays(-1);
                model.Logs = repo.Query<Log>(x =>
                    !x.Logger.Equals(nameof(DaoLog)) && x.CreateTime >= dateLower)
                    .OrderByDescending(x => x.CreateTime).ToList();

            }

            return View(model);
        }
    }
}