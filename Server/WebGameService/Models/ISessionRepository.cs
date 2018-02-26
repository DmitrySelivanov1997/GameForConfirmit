using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.OData.Query;

namespace WebGameService.Models
{
    public interface ISessionRepository
    {
        List<GameSessionStatistic> GetAll(ODataQueryOptions<GameSessionStatistic> queryOptions);
        Task Add(GameSessionStatistic statistic);
        int GetCountOfEntries(ODataQueryOptions<GameSessionStatistic> queryOptions);
    }
}