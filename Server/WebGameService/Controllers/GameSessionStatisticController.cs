using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebGameService.Models;
using System.Web.Http.OData;
using System.Web.OData;

namespace WebGameService.Controllers
{
    //[RoutePrefix("statistics")]
    //public class GameSessionStatisticController : EntitySetController<GameSessionStatistic, int>
    //{
    //    private DataBaseManager DbManager = new DataBaseManager();
    //    [HttpGet]
    //    public SingleResult<GameSessionStatistic> GetAll(int key)
    //    {
    //        using (var db = new GameSessionStatisticContext())
    //        {
    //            IQueryable<GameSessionStatistic> result = db.GameSessionStatistics.Where(p => p.Id == key);
    //            return SingleResult.Create(result);
    //        }
    //    }
    //    public override IQueryable<GameSessionStatistic> Get(int count)
    //    {
    //        using (var db = new GameSessionStatisticContext())
    //        {
    //            return db.GameSessionStatistics.Where();
    //        }
    //    }

    //}
    public class GameSessionStatisticController : ApiController
    {
        [System.Web.Http.OData.EnableQuery]
        [HttpGet]
        public IQueryable<GameSessionStatistic> GetData()
        {
            //using (var db = new GameSessionStatisticContext())
            //{
            var db = new GameSessionStatisticContext();
                return db.GameSessionStatistics.AsQueryable();
            //}
        }
    }
}
