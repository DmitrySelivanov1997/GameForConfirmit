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
        public void Index(GameSessionStatistic gameSessionStatistic)
        {
            using (var db = new GameSessionStatisticContext())

            
{
                db.GameSessionStatistics.Add(gameSessionStatistic);
                db.SaveChanges();
            }
        }
    }
}
