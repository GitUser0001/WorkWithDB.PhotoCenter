using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.PostgreSQL.Infrastructure;

namespace WorkWithDB.DAL.PostgreSQL.Repository
{
    internal class GoodsSoldRepository : BaseRepository<int, GoodsSold>, IGoodsSoldRepository
    {
        public GoodsSoldRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(GoodsSold entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into goods_sold (goods_id,service_id,sold_count,price)
                    values (@goods_id,@service_id,@sold_count,@price) RETURNING id",
                    new SqlParameters                    
                    {                                   
                        {"goods_id", entity.GoodsID},                    
                        {"service_id", entity.ServiceID},                    
                        {"sold_count", entity.SoldCount},    
                        {"price", entity.Price}                
                    });

            return entity.Id;
        }

        public override bool Update(GoodsSold entity)
        {
            var res = base.ExecuteNonQuery(
            @"update goods_sold set goods_id=@goods_id,service_id=@service_id,sold_count=@sold_count,price=@price
                WHERE id=@id",
                new SqlParameters
                    {
                        {"id", entity.Id},
                        {"goods_id", entity.GoodsID},                    
                        {"service_id", entity.ServiceID},                    
                        {"sold_count", entity.SoldCount},    
                        {"price", entity.Price}      
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from goods_sold");
        }

        public GoodsSold GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from goods_sold where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
            "delete from goods_sold where id = @id",
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

        public bool Delete(int goodsId, int serviceId)
        {
            var res = base.ExecuteNonQuery(
                        "delete from goods_sold where service_id = @serviceId AND goods_id = @goodsId",
                        new SqlParameters()
                        {                             
                            {"serviceId", goodsId},
                            {"goodsId", serviceId}
                        });

            if (res > 1)
            {
                throw new InvalidOperationException("Multiple rows deleted by single delete query");
            }

            return res == 1;
        }

        public IList<GoodsSold> GetAll()
        {
            return base.ExecuteSelect("select * from goods_sold");
        }

        protected override GoodsSold DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new GoodsSold
            {
                Id = (int)reader["id"],
                ServiceID = (int)reader["service_id"],
                GoodsID = (int)reader["goods_id"],
                Price = (int)reader["price"],
                SoldCount = (int)reader["sold_count"]
            };
        }
    }
}
