﻿using System.Web;
using System.Web.Mvc;
using WebGameService.Models;

namespace WebGameService
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
