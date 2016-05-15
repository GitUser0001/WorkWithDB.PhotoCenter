using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = WorkWithDB.DAL.Entity.Entities;

using System.Net;
using RestSharp;

namespace Goods.Tests
{
    [TestClass]
    public class TestApi
    {
        private static string webAPIUriSimple = "http://localhost:53455/";
        private static string webAPIUriAPI = "http://localhost:53455/api/";

        private RestClient client = new RestClient(webAPIUriAPI);
        private RestClient clientOAuth = new RestClient(webAPIUriSimple);

        public TestApi()
        {
            var request = new RestRequest("api/Auth/Login", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { Nick = "Dan", Password = "123"});
            IRestResponse<object> response = client.Execute<object>(request);

            var a = response.ToString();

            
        }

        [TestMethod]
        public void TestGETGoods()
        {
            var request = new RestRequest("Medication/GetAll/", Method.GET);
            
            IRestResponse<List<Model.Goods>> response = client.Execute<List<Model.Goods>>(request);
            List<Model.Goods> medics = response.Data;

            Assert.AreEqual(medics.Count, 3);
        }
    }
}
