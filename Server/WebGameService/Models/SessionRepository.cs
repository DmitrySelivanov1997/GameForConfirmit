using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http.OData.Query;

namespace WebGameService.Models
{
    public class SessionRepository: ISessionRepository
    {
        private GameSessionStatisticContext _context;

        public SessionRepository(GameSessionStatisticContext context)
        {
            _context = context;
        }
        public List<GameSessionStatistic> GetAll(ODataQueryOptions<GameSessionStatistic> queryOptions)
        {
            return queryOptions.ApplyTo(_context.GameSessionStatistics).OfType<GameSessionStatistic>().ToList();
        }


        public int GetCountOfEntries(ODataQueryOptions<GameSessionStatistic> queryOptions)
        {
            return queryOptions.ApplyTo(_context.GameSessionStatistics).OfType<GameSessionStatistic>().Count();
        }
        public void Add(GameSessionStatistic statistic)
        {
            _context.GameSessionStatistics.Add(statistic);
            _context.SaveChanges();
        }
    }
}