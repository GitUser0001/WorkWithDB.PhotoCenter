using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Monads.NET;
using WebApp.Abstract.Security;
using WorkWithDB.DAL.Abstract;
using WebApp.Api.Models.Model;

namespace WebApp.BL.Security
{
    public class CredentialsChecker : ICredentialsChecker
    {
        private readonly UsersRepository _userRepository;

        public CredentialsChecker(UsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int? CheckUserExist(string userName, string userPassword)
        {
            var user = _userRepository.GetByLoginPassword(userName, userPassword);
            if (user != null)
                return user.Id;
            else
                return null;
        }
    }
}
