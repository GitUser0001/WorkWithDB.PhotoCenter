using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.PostgreSQL.Infrastructure;

namespace WorkWithDB.DAL.PostgreSQL.Repository
{
    internal class DeliveryGoodsRepository : BaseRepository<int, DeliveryGoods>, IDeliveryGoodsRepository
    {
        public DeliveryGoodsRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }


        public override int Save(DeliveryGoods entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into delivery_goods (delivery_id,goods_id,count,price)
                    values (@delivery_id,@goods_id,@count,@price) RETURNING id",
                    new SqlParameters                    
                    {          
                        {"delivery_id", entity.Delivery.Id},                    
                        {"goods_id", entity.Goods.Id},                    
                        {"count", entity.Count},    
                        {"price", entity.Price}                               
                    });

            return entity.Id;
        }

        public override bool Update(DeliveryGoods entity)
        {
            var res = base.ExecuteNonQuery(
            @"update delivery_goods set delivery_id=@delivery_id,goods_id=@goods_id,count=@count,price=@price
                WHERE id=@id",
                new SqlParameters
                        {
                            {"id", entity.Id},
                            {"delivery_id", entity.Delivery.Id},                    
                            {"goods_id", entity.Goods.Id},                    
                            {"count", entity.Count},    
                            {"price", entity.Price}  
                        });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from delivery_goods");
        }

        public DeliveryGoods GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from delivery_goods where id = @id",
                    new SqlParameters()                        
                    {
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
            "delete from delivery_goods where id = @id",
            new SqlParameters()
                        {                             
                            {"id", id}
                        });

            if (res > 1)
            {
                throw new InvalidOperationException("Multiple rows deleted by single delete query");
            }

            return res == 1;
        }

        public IList<DeliveryGoods> GetAll()
        {
            return base.ExecuteSelect("select * from delivery_goods");
        }

        protected override DeliveryGoods DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {            
                return new DeliveryGoods
                {
                    Id = (int)reader["id"],
                    Delivery = unitOfWork.DeliveryRepository.GetByID((int)reader["delivery_id"]),
                    Goods = unitOfWork.GoodsRepository.GetByID((int)reader["goods_id"]),
                    Count = (int)reader["count"],
                    Price = (float)reader["price"]
                };
            }
        }
    }
}
