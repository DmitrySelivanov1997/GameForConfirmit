using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests
{
    /// <summary>
    /// Testing AlgorithmController functionality like 
    /// get/set algorithm to use
    /// </summary>
    [TestClass]
    public class AlgorithmControllerTests
    {
        private TestWebCaller WebCaller = new TestWebCaller();

        [TestMethod]
        public async Task SetAndCheckWhiteAlgorithmName()
        {
            var result = await SetAndGetAlgorithm("white", "Algoritm.dll", "\"Algoritm.Algoritm1\"");
            Assert.IsTrue(result);

        }
        [TestMethod]
        public async Task SetAndCheckBlackAlgorithmName()
        {
            var result = await SetAndGetAlgorithm("black", "Algoritm.dll", "\"Algoritm.Algoritm1\"");
            Assert.IsTrue(result);
        }

        private async Task<bool> SetAndGetAlgorithm(string algType,string _assembly, string expected)
        {
            var fileName = Path.GetFullPath(_assembly);
            var assembly = File.ReadAllBytes(fileName);
            await WebCaller.PostAlgorithmToTheServer(assembly, algType);
            var result = await WebCaller.GetAlgoritmnameFromTheServer(algType);
            return result == expected;
        }
    }
}
