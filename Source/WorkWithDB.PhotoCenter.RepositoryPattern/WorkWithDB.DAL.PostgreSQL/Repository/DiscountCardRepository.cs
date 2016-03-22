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
    internal class DiscountCardRepository : BaseRepository<int, DiscountCard>, IDiscountCardRepository
    {
        private readonly string tabelName = "dicount_card";

        public DiscountCardRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(DiscountCard entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,type_name,discount,code,is_personal)
                    values (@id,@type_name,@discount,@code,@is_personal) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"type_name", entity.TypeName},                    
                        {"discount", entity.Discount},    
                        {"code", entity.Code},                    
                        {"is_personal", entity.IsPersonal}           
                    });
        }

        public override bool Update(DiscountCard entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,type_name=@type_name,discount=@discount,code=@code,is_personal=@is_personal",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"type_name", entity.TypeName},                    
                        {"discount", entity.Discount},    
                        {"code", entity.Code},                    
                        {"is_personal", entity.IsPersonal}     
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

        public DiscountCard GetByID(int id)
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

        public IList<DiscountCard> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override DiscountCard DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new DiscountCard
            {
                Id = (int)reader["id"],
                Code = (int)reader["code"],
                TypeName = (string)reader["type_name"],
                Discount = (int)reader["discount"],
                IsPersonal = (bool)reader["is_personal"]
            };
        }
    }
}
