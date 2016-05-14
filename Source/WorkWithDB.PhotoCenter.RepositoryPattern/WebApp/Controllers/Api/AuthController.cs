using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApp.Abstract.Security;
using WebApp.Api.Models.Model;
using WebApp.Api.Models.Requests;
using WebApp.Api.Models.Responces;
using WebApp.BL.Security;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.PostgreSQL;

namespace WebApp.Controllers.Api
{
        [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        private readonly ICredentialsChecker _credentialsChecker;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private UsersRepository _usersRepository;

        public AuthController()
        {
            _usersRepository = DataHolder.UsersRepo;
            _credentialsChecker = DataHolder.CredentialsChecker;
            _accessTokenGenerator = DataHolder.TokenGenerator;
        }

        [HttpPost]
        [Route("Login")]
        public AuthResult Login(LoginModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var userId = _credentialsChecker.CheckUserExist(model.Nick, model.Password);
            if (userId.HasValue)
            {
                var token = _accessTokenGenerator.GenerateToken(userId.Value, model.Nick);

                var user = _usersRepository.GetById(userId.Value);                               

                return new AuthResult { Token = token, Message = "Ok", User = user };
            }

            return new AuthResult { Message = "Unauthorized!" };
        }


        [HttpPost]
        [Route("Register")]
        public AuthResult Register(RegisterModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (String.IsNullOrWhiteSpace(model.Nick) ||
                string.IsNullOrWhiteSpace(model.Password))
                return new AuthResult { Message = "Incorrect fields!" };

            var user = new User { Nick = model.Nick, Password = model.Password };
            var userId = _usersRepository.Insert(user);
            if (userId > 0)
            {
                var token = _accessTokenGenerator.GenerateToken(userId, model.Nick);

                var createdUser = _usersRepository.GetById(userId);

                return new AuthResult { Token = token, Message = "Ok", User = createdUser };
            }

            return new AuthResult { Message = "Can't save user!" };
        }
    }
}
