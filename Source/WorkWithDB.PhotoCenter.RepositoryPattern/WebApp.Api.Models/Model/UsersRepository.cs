using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Api.Models.Model
{
    class UsersRepository
    {
        private List<User> users = new List<User>() 
        {
            new User() { Id=1, Nick="Dan", Password="123"},
            new User() { Id=2, Nick="Test", Password="1"}
        };

        public List<User> GetAll
        {
            get
            {
                return users;
            }
        }

        public User GetById(int id)
        {
            foreach (var user in users)
            {
                if (user.Id == id)
                {
                    return user;
                }
            }

            return null;
        }

        public User GetByLoginPassword(string login, string password)
        {
            foreach (var user in users)
            {
                if (user.Nick == login && user.Password == password)
                    return user;
            }

            return null;
        }
    }
}
