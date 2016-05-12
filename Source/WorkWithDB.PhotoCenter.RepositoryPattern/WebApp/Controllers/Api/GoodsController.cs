using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApp.Abstract.Security;
using WebApp.Api.Models.Responces;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.PostgreSQL;
using Model = WorkWithDB.DAL.Entity.Entities;


namespace WebApp.Controllers.Api
{
    public class GoodsController : ApiController
    {
        private readonly IAccessTokenValidator _tokenValidator;
        private readonly IGoodsRepository _goodsRepository;

        public GoodsController()
        {
            UnitOfWorkFactory.__Initialize(() => new UnitOfWork());
            _goodsRepository = UnitOfWorkFactory.CreateInstance().GoodsRepository;
        }

        [HttpGet]
        public Result<IList<Model.Goods>> GetAll(string token)
        {
            //var user =
            //    _tokenValidator.ValidateToken(token);

            //if (user == null)
            //    return Result<IList<Model.Goods>>.Unauthorized;

            var a = _goodsRepository.GetAll();
            return new Result<IList<Model.Goods>>(a);
        }
    }
}