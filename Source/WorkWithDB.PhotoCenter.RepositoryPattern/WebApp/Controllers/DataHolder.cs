using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Abstract.Security;
using WebApp.Api.Models.Model;
using WebApp.BL.Security;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.PostgreSQL;

namespace WebApp.Controllers
{
    internal static class DataHolder
    {
        private static AccessTokenManager _tokenValidator;
        public static IAccessTokenValidator TokenValidator
        {
            get
            {
                return _tokenValidator ?? (_tokenValidator = new AccessTokenManager());
            }
        }

        public static IAccessTokenGenerator TokenGenerator
        {
            get
            {
                return _tokenValidator ?? (_tokenValidator = new AccessTokenManager());
            }
        }
        
        private static IUnitOfWork _scope;
        public static IUnitOfWork Scope
        {
            get
            {
                return _scope ?? (_scope = UnitOfWorkFactory.CreateInstance());
            }
        }

        private static UsersRepository _usersRepo;
        public static UsersRepository UsersRepo
        {
            get
            {
                return _usersRepo ?? (_usersRepo = new UsersRepository());
            }
        }

        private static CredentialsChecker _credChecker;
        public static CredentialsChecker CredentialsChecker
        {
            get
            {
                return _credChecker ?? (_credChecker = new CredentialsChecker(UsersRepo));
            }
        }
    }
}