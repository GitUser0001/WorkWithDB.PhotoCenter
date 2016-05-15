using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Api.Models.Responces;
using Model = WorkWithDB.DAL.Entity.Entities;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using WebApp.Models;

namespace WebApp.WebControllers
{
    public class GoodsController : Controller
    {
        private string webAPIUriAPI = "http://localhost:62106/";
        private RestClient client;

        public GoodsController()
        {
            client = new RestClient(webAPIUriAPI);
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var token = Request.Cookies.Get("token");

            if (token != null)
            {
                var request = new RestRequest("api/Goods/All", Method.GET);
                //request.RequestFormat = DataFormat.Json;
                request.AddQueryParameter(token.Name, token.Value);

                IRestResponse<Result<IList<Model.Goods>>> response = client.Execute<Result<IList<Model.Goods>>>(request);

                var res = new JavaScriptSerializer().Deserialize<Result<IList<Model.Goods>>>(response.Content);


                if (res.HasError)
                {
                    return View("Error", res.GetError());
                }
                                
                return View(res.Value);
            }

            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }

        [HttpGet]
        public ActionResult Create()
        {
            var token = Request.Cookies.Get("token");

            if (token != null)
            {
                var request = new RestRequest("api/Auth/Validate", Method.GET);
                request.AddQueryParameter(token.Name, token.Value);

                IRestResponse<AuthResult> response = client.Execute<AuthResult>(request);
                //var res = new JavaScriptSerializer().Deserialize<AuthResult>(response.Content);

                if (response.Data.Message != "Ok")
                {
                    return View("Error", new Result() { ErrorMessage = response.Data.Message });
                }

                return View();
            }
            
            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }

        [HttpPost]
        public ActionResult Create(Model.Goods goods)
        {
            var token = Request.Cookies.Get("token");

            if (token != null && goods != null)
            {
                var request = new RestRequest("api/Goods/Save", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddQueryParameter(token.Name, token.Value);
                request.AddBody(goods);

                IRestResponse<Result<int>> response = client.Execute<Result<int>>(request);

                
                if (response.Data.HasError)
                {
                    return View("Error", response.Data.GetError());
                }

                Response.Redirect("GetAll");
                return null;
            }

            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }

        [HttpGet]
        public ActionResult Count()
        {
            var token = Request.Cookies.Get("token");

            if (token != null)
            {
                var request = new RestRequest("api/Goods/Count", Method.GET);
                request.AddQueryParameter(token.Name, token.Value);

                IRestResponse<Result<int>> response = client.Execute<Result<int>>(request);

                if (response.Data.HasError)
                {
                    return View("Error", response.Data.GetError());
                }

                return View(new GoodsCountViewModel(response.Data.Value));
            }

            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }

        [HttpGet]
        public ActionResult CountBy([System.Web.Http.FromUri] string country)
        {
            var token = Request.Cookies.Get("token");

            if (token != null && !String.IsNullOrWhiteSpace(country))
            {
                var request = new RestRequest("api/Goods/Count", Method.GET);
                request.AddQueryParameter(token.Name, token.Value);
                request.AddQueryParameter("country", country);

                IRestResponse<Result<int>> response = client.Execute<Result<int>>(request);

                if (response.Data.HasError)
                {
                    return View("Error", response.Data.GetError());
                }

                return View("Count",new GoodsCountViewModel(response.Data.Value, country));
            }

            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }

        [HttpGet]
        public ActionResult Get([System.Web.Http.FromUri] int id)
        {
            var token = Request.Cookies.Get("token");

            if (token != null)
            {
                var request = new RestRequest("api/Goods/Get", Method.GET);
                request.AddQueryParameter(token.Name, token.Value);
                request.AddQueryParameter("id", id.ToString());

                IRestResponse<Result<Model.Goods>> response = client.Execute<Result<Model.Goods>>(request);

                if (response.Data.HasError)
                {
                    return View("Error", response.Data.GetError());
                }

                return View(response.Data.Value);
            }

            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }

        [HttpGet]
        public ActionResult Delete([System.Web.Http.FromUri] int id)
        {
            var token = Request.Cookies.Get("token");

            if (token != null)
            {
                var request = new RestRequest("api/Goods/Delete", Method.GET);
                request.AddQueryParameter(token.Name, token.Value);
                request.AddQueryParameter("id", id.ToString());

                IRestResponse<Result<bool>> response = client.Execute<Result<bool>>(request);

                if (response.Data.HasError)
                {
                    return View("Error", response.Data.GetError());
                }

                return View("Error", new Result() { InfoMessage = "Satus " + response.Data.Value.ToString() });
            }

            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }

        [HttpGet]
        public ActionResult Update([System.Web.Http.FromUri] int id)
        {
            var token = Request.Cookies.Get("token");

            if (token != null)
            {
                var request = new RestRequest("api/Goods/Get", Method.GET);
                request.AddQueryParameter(token.Name, token.Value);
                request.AddQueryParameter("id", id.ToString());

                IRestResponse<Result<Model.Goods>> response = client.Execute<Result<Model.Goods>>(request);

                if (response.Data.HasError)
                {
                    return View("Error", response.Data.GetError());
                }

                response.Data.Value.Id = id;

                return View(response.Data.Value);
            }

            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }

        [HttpPost]
        public ActionResult Update([System.Web.Http.FromBody] Model.Goods goods)
        {
            var token = Request.Cookies.Get("token");

            if (token != null)
            {
                var request = new RestRequest("api/Goods/Put", Method.PUT);
                request.RequestFormat = DataFormat.Json;
                request.AddQueryParameter(token.Name, token.Value);
                request.AddBody(goods);

                IRestResponse<Result<bool>> response = client.Execute<Result<bool>>(request);

                if (response.Data.HasError)
                {
                    return View("Error", response.Data.GetError());
                }

                return View("Error", new Result() { InfoMessage = "Satus " + response.Data.Value.ToString() });
            }

            return View("Error", new Result() { ErrorMessage = "Unauthorized" });
        }
    }
}