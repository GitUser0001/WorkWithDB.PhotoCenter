using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RestSharp;
using WebApp.Api.Models.Requests;
using WebApp.Api.Models.Responces;

namespace WebApp.WebControllers
{
    public class WebAccountController : Controller
    {
        private string webAPIUriAPI = "http://localhost:62106/";
        private RestClient client;

        public WebAccountController()
        {
            client = new RestClient(webAPIUriAPI);
        }
            
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel userModel)
        {
            var request = new RestRequest("api/Auth/Login", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(userModel);

            IRestResponse<AuthResult> response = client.Execute<AuthResult>(request);

            if (response.Data.Message == "Ok")
            {
                Response.Cookies.Add(new System.Web.HttpCookie("token", response.Data.Token));                
                return View("../Home/Index");
            }

            return View("Error", response.Data);

            //var i = Request.Cookies.Count;
            //var a = Request.Cookies.AllKeys;
            //var b = Request.Cookies.Get("token");

            //return View("Views/Home/Index.cshtml");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            var request = new RestRequest("api/Auth/Register", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(model);

            IRestResponse<AuthResult> response = client.Execute<AuthResult>(request);

            if (response.Data.Message == "Ok")
            {
                Response.Cookies.Add(new System.Web.HttpCookie("token", response.Data.Token));
                return View("../Home/Index");
            }

            return View("Error", response.Data);


            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}