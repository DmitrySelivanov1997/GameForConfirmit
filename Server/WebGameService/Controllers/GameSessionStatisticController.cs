using System.Collections.Generic;
using System.Web.Http;
using WebGameService.Models;
using System.Web.Http.OData.Query;

namespace WebGameService.Controllers
{
    [RoutePrefix("api/statistic")]
    public class GameSessionStatisticController : ApiController
    {
        private readonly ISessionRepository _repository;

        public GameSessionStatisticController(ISessionRepository repository)
        {
            _repository = repository;
        }
        [Route("")]
        [HttpGet]
        public List<GameSessionStatistic> GetData(ODataQueryOptions<GameSessionStatistic> queryOptions)
        {
            return _repository.GetAll(queryOptions);
        }
        [Route("count")]
        [HttpGet]
        public int GetCount(ODataQueryOptions<GameSessionStatistic> queryOptions)
        {
            var count = _repository.GetCountOfEntries(queryOptions); 
            return count;
        }
    }
    
}
