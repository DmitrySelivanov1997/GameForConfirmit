using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WebGameService.Models;

namespace IntegrationTests
{
    /// <summary>
    /// Summary description for SessionStatisticController
    /// </summary>
    [TestClass]
    public class SessionStatisticControllerTests
    {
        private readonly string _appPath = "http://co-yar-ws100:8080/";

        private async Task<List<GameSessionStatistic>> GetSortedList(string querryOption)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(_appPath + "/api/statistic"+querryOption);
                var actual = JsonConvert.DeserializeObject<List<GameSessionStatistic>>(response);
                return actual;
            }

        }

        [TestMethod]
        public void CanSortByIdDesc()
        {
            var actual = GetSortedList("?$orderby=Id desc").Result;
            var expected = actual.OrderByDescending(x => x.Id).ToList();
            CollectionAssert.AreEqual(expected,actual);
        }
        [TestMethod]
        public void CanSortByIdAsc()
        {
            var actual = GetSortedList("?$orderby=Id asc").Result;
            var expected = actual.OrderBy(x => x.Id).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
