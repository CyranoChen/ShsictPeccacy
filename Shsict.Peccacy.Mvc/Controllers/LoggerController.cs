using System;
using System.Linq;
using System.Web.Mvc;
using Shsict.Peccacy.Mvc.Models;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Logger;

namespace Shsict.Peccacy.Mvc.Controllers
{
    public class LoggerController : Controller
    {
        // GET: Logger
        public ActionResult Index()
        {
            var model = new ConsoleModels.LogListDto();

            using (IRepository repo = new Repository())
            {
                // 非数据库操作日志（7天内）
                var dateLower = DateTime.Now.AddDays(-7);
                model.Logs = repo.Query<Log>(x =>
                    !x.Logger.Equals(nameof(DaoLog)) && x.CreateTime >= dateLower)
                    .OrderByDescending(x => x.CreateTime).ToList();

            }

            return View(model);
        }
    }
}