﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WebGameService.Models;

namespace WebGameService.Controllers
{
    public class DbController : ApiController
    {
        private DataBaseManager DbManager = new DataBaseManager();
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/db/getstatsbyorder/{pageNumber}/{orderClass}/{orderType}")]
        public int GetStatsByOrder(int pageNumber, string orderClass, string orderType)
        {
            return 42;
        }
        [System.Web.Http.HttpGet]
        public List<GameSessionStatistic> GetAllDataFromDb(int id)
        {

            return DbManager.GetRangeOfObjects((id-1)*25); // Id is the number of page and here is also means the first element in db.List
        }
    }
}
