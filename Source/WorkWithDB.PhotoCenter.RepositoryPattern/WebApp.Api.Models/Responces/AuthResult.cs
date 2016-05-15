using WebApp.Api.Models.Model;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WebApp.Api.Models.Responces
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public User User{ get; set; }
    }
}
