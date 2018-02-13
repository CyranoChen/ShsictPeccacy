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
                // 数据库业务操作日志与异常（7天内）
                var dateLower = DateTime.Now.AddDays(-7);
                model.Logs = repo.Query<Log>(x =>
                    (x.Logger.Equals(nameof(UserLog)) || x.IsException) && x.CreateTime >= dateLower)
                    .OrderByDescending(x => x.CreateTime).ToList();

            }

            return View(model);
        }
    }
}