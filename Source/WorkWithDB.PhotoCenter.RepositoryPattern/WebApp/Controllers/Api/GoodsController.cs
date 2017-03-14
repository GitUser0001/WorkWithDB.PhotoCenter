using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApp.Abstract.Security;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.PostgreSQL;
using WebApp.BL.Security;
using Model = WorkWithDB.DAL.Entity.Entities;
using WebApp.Api.Models.Responces;

namespace WebApp.Controllers.Api
{
    [RoutePrefix("api/Goods")]
    public class GoodsController : ApiController
    {
        private readonly IAccessTokenValidator _tokenValidator;
        private readonly IGoodsRepository _goodsRepository;

        public GoodsController()
        {
            _goodsRepository = DataHolder.Scope.GoodsRepository;
            _tokenValidator = DataHolder.TokenValidator;
        }

        [HttpGet]
        [Route("All")]
        public Result<IList<Model.Goods>> GetAll([FromUri]string token)
        {
            var user =
                _tokenValidator.ValidateToken(token);

            if (user == null)
                return Result<IList<Model.Goods>>.Unauthorized;


            Result<IList<Model.Goods>> result;
            try
            {
                result = new Result<IList<Model.Goods>>(_goodsRepository.GetAll());
            }
            catch (Exception)
            {
                result = Result<IList<Model.Goods>>.NotExist;
            }            

            return result;
        }

        [HttpPost]
        [Route("Save")]
        public Result<int> Save([FromUri]string token, [FromBody] Model.Goods goods)
        {
            var user =
                _tokenValidator.ValidateToken(token);

            if (user == null || user.Id == 0)
                return Result<int>.Unauthorized;

            if (String.IsNullOrWhiteSpace(goods.Name))
            {
                var existingGoods = _goodsRepository.GetByID(goods.Id);
                if (existingGoods == null)
                    return new Result<int> { ErrorMessage = "404 Incorrect ID", HasError = true };
            }


            int goodsId;

            using (var scope = UnitOfWorkFactory.CreateInstance())
            {
                goodsId = scope.GoodsRepository.SaveOrUpdate(goods);
                scope.Commit();
            }

            return goodsId;
        }

        [HttpGet]
        [Route("Count")]
        public Result<int> Count([FromUri] string token)
        {
            var user =
                _tokenValidator.ValidateToken(token);

            if (user == null)
                return Result<int>.Unauthorized;

            return _goodsRepository.GetCount();
        }

        [HttpGet]
        [Route("Count")]
        public Result<int> CountByCountry([FromUri] string token, [FromUri] string country)
        {
            var user =
                _tokenValidator.ValidateToken(token);

            if (user == null)
                return Result<int>.Unauthorized;

            return _goodsRepository.GetCountByCountry(country);
        }

        [HttpGet]
        [Route("Get")]
        public Result<Model.Goods> GetByID([FromUri]string token, [FromUri]int id)
        {
            var user =
                _tokenValidator.ValidateToken(token);

            if (user == null)
                return Result<Model.Goods>.Unauthorized;
         

            var goods = _goodsRepository.GetByID(id);

            if (goods == null)
            {
                return Result<Model.Goods>.NotExist;
            }

            return new Result<Model.Goods>(goods);
        }

        [HttpGet]
        [Route("Delete")]
        public Result<bool> Delete([FromUri] string token, [FromUri] int id)
        {
            var user =
                _tokenValidator.ValidateToken(token);

            if (user == null)
                return Result<bool>.Unauthorized;

            bool result;

            using (var scope = UnitOfWorkFactory.CreateInstance())
            {
                result = scope.GoodsRepository.Delete(id); ;
                scope.Commit();
            }

            return result;
        }

        [HttpPut]
        [Route("Put")]
        public Result<bool> Update([FromUri] string token, [FromBody] Model.Goods goods)
        {
            var user =
                _tokenValidator.ValidateToken(token);

            if (user == null)
                return Result<bool>.Unauthorized;

            if (goods.Id == 0)
            {
                return new Result<bool> { ErrorMessage = "404 Incorrect ID", HasError = true };
            }
            
            bool res;

            using (var scope = UnitOfWorkFactory.CreateInstance())
            {
                res = scope.GoodsRepository.Update(goods);
                scope.Commit();
            }

            return res;
        }
    }
}