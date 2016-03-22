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
        private readonly string tabelName = "goods_sold";

        public GoodsSoldRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(GoodsSold entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (goods_id,service_id,sold_count,price)
                    values (@goods_id,@service_id,@sold_count,@price) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"goods_id", entity.GoodsID},                    
                        {"service_id", entity.ServiceID},                    
                        {"sold_count", entity.SoldCount},    
                        {"price", entity.Price}                
                    });
        }

        public override bool Update(GoodsSold entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set goods_id=@goods_id,service_id=@service_id,sold_count=@sold_count,price=@price",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"goods_id", entity.GoodsID},                    
                        {"service_id", entity.ServiceID},                    
                        {"sold_count", entity.SoldCount},    
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

        public GoodsSold GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from @tableName where service_id = @id",
                    new SqlParameters()                        
                    {
                        {"tableName", tabelName},
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int goodsId, int serviceId)
        {
            var res = base.ExecuteNonQuery(
                        "delete from @tableName where service_id = @serviceId AND goods_id = @goodsId",
                        new SqlParameters()
                        {
                            {"tableName", tabelName},
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
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override GoodsSold DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new GoodsSold
            {
                ServiceID = (int)reader["service_id"],
                GoodsID = (int)reader["goods_id"],
                Price = (int)reader["price"],
                SoldCount = (int)reader["sold_count"]
            };
        }
    }
}
