﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using WebGameService.Models;

namespace WebGameService.Controllers
{
    public class DbController : ApiController
    {
        private DataBaseManager DbManager = new DataBaseManager();
        [System.Web.Http.HttpGet]
        public void GetDataFromDb()
        {
             DbManager.GetWholeInformation();
        }
    }
}
