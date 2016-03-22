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
    internal class ProviderRepository : BaseRepository<int, Provider>, IProviderRepository
    {
        private readonly string tabelName = "provicer";

        public ProviderRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(Provider entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,name,mobile_number,email,owner_info)
                    values (@id,@name,@mobile_number,@email,@owner_info) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"mobile_number", entity.MobileNumber},    
                        {"email", entity.Email}, 
                        {"owner_info", entity.OwnerInfo}  
                    });
        }

        public override bool Update(Provider entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,name=@name,mobile_number=@mobile_number,email=@email,owner_info=@owner_info",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"mobile_number", entity.MobileNumber},    
                        {"email", entity.Email},                    
                        {"owner_info", entity.OwnerInfo}  
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

        public Provider GetByID(int id)
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

        public IList<Provider> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override Provider DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new Provider
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                MobileNumber = (int)reader["mobile_number"],
                Email = (string)reader["email"],
                OwnerInfo = (string)reader["owner_info"]          
            };
        }
    }
}
