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
        private readonly string _appPath = "http://co-yar-ws100:8080/";
        private async Task PostAlgorithmToTheServer(byte[] asm, string id)
        {
            using (var client = new HttpClient())
            {
                var byteArrayContent = new ByteArrayContent(asm);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/bson"));
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/bson");
                await client.PostAsync(_appPath + "api/algorithm/" + id, byteArrayContent);
            }
        }
        private async Task<string> GetAlgoritmnameFromTheServer(string id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(_appPath + "api/algorithm/" + id);
                return response;
            }
        }

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
            await PostAlgorithmToTheServer(assembly, algType);
            var result = await GetAlgoritmnameFromTheServer(algType);
            return result == expected;
        }
    }
}
