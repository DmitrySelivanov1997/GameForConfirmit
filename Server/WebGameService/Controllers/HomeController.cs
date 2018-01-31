using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGameService.Models;

namespace WebGameService.Controllers
{
    public class HomeController : Controller
    {
        private GameSessionStatisticContext db = new GameSessionStatisticContext();
        public ActionResult Index()
        {
            return View(db.GameSessionStatistics);
        }
        public ActionResult Create()
        {
            db.GameSessionStatistics.Add(new GameSessionStatistic());
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
