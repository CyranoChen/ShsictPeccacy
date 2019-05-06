using System;
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
                // 违规集卡数据同步（6月内）
                var dateLower = DateTime.Now.AddMonths(-6);
                model.TruckCamRecords = repo.Query<TruckCamRecord>(x => x.PicTime >= dateLower)
                    .OrderByDescending(x => x.PicTime).ToList();
            }

            return View(model);
        }
    }
}