using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebGameService.Models;
using InterfaceLibrary;

namespace WebGameService.Controllers
{
    public class AlgorithmController : ApiController
    {

        [System.Web.Http.HttpGet]
        public string GetAlgorithmName(string id)
        {
            switch (id)
            {
                case "white":
                    return AlgorithmContainer.AlgorithmWhite.GetType().FullName;
                case "black":
                    return AlgorithmContainer.AlgorithmBlack.GetType().FullName;
            }
            return "Failed to get algorithm name";
        }
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> PostAlgoritm(string id)
        {
            byte[] bytes = await Request.Content.ReadAsByteArrayAsync();
            var asm = Assembly.Load(bytes);
            var types = asm.GetTypes();
            if (types.Any(type => type.GetInterface("IAlgorithm") != null))
            {
                foreach (var type in types)
                {
                    if (type.GetInterface("IAlgorithm") == null) continue;
                    if (id == "white")
                        AlgorithmContainer.AlgorithmWhite = (IAlgorithm)Activator.CreateInstance(type);
                    else
                        AlgorithmContainer.AlgorithmBlack = (IAlgorithm)Activator.CreateInstance(type);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.NotModified);
        }

    }
}
