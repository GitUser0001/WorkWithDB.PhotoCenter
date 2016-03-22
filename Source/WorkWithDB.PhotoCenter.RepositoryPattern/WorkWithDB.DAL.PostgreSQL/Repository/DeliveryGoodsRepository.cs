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
        private readonly string tabelName = "delivery_goods";

        public DeliveryGoodsRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }


        public override int Save(DeliveryGoods entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (delivery_id,goods_id,count,price)
                    values (@delivery_id,@goods_id,@count,@price) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"delivery_id", entity.Delivery.Id},                    
                        {"goods_id", entity.Goods.Id},                    
                        {"count", entity.Count},    
                        {"price", entity.Price}                               
                    });
        }

        public override bool Update(DeliveryGoods entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set delivery_id=@delivery_id,goods_id=@goods_id,count=@count,price=@price",
                new SqlParameters
                        {
                            {"tableName", tabelName},
                            {"delivery_id", entity.Delivery.Id},                    
                            {"goods_id", entity.Goods.Id},                    
                            {"count", entity.Count},    
                            {"price", entity.Price}  
                        });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>(
                        "select count(*) from @tableName",
                        new SqlParameters 
                        { 
                            {"tableName", tabelName}
                        });
        }

        public DeliveryGoods GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from @tableName where id = @id",
                    new SqlParameters()                        
                    {
                        {"tableName", tabelName},
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
            "delete from @tableName where id = @id",
            new SqlParameters()
                        {
                            {"tableName", tabelName},
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
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
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
