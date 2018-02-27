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
        private TestWebCaller WebCaller = new TestWebCaller();

        #region Sorting
        [TestMethod]
        public void CanSortByIdDesc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=Id desc").Result;
            var expected = actual.OrderByDescending(x => x.Id).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CanSortByIdAsc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=Id asc").Result;
            var expected = actual.OrderBy(x => x.Id).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CanSortByStartTimeAsc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=GameStartTime asc").Result;
            var expected = actual.OrderBy(x => x.GameStartTime).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CanSortByDurationAsc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=GameDuration asc").Result;
            var expected = actual.OrderBy(x => x.GameDuration).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CanSortByTurnNumberAsc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=TurnsNumber asc").Result;
            var expected = actual.OrderBy(x => x.TurnsNumber).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CanSortByMapSizeAsc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=MapSize asc").Result;
            var expected = actual.OrderBy(x => x.MapSize).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CanSortByGameResultAsc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=GameResult asc").Result;
            var expected = actual.OrderBy(x => x.GameResult).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CanSortByWhiteAlgorithmNameAsc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=WhiteAlgorithmName asc").Result;
            var expected = actual.OrderBy(x => x.WhiteAlgorithmName).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void CanSortByBlackAlgorithmNameAsc()
        {
            var actual = WebCaller.GetSortedList("?$orderby=BlackAlgorithmName asc").Result;
            var expected = actual.OrderBy(x => x.BlackAlgorithmName).ToList();
            CollectionAssert.AreEqual(expected, actual);
        }
        #endregion

        #region Paging

        [TestMethod]
        public void CanGet25Entries()
        {
            var actual = WebCaller.GetSortedList("?$skip=0 &$top=25").Result.Count;
            var expected = 25;
            Assert.AreEqual(expected, actual);
        }
        #endregion

    }
}
