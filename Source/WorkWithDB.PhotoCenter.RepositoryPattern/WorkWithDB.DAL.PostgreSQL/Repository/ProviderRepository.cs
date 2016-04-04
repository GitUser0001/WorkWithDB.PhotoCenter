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
        public ProviderRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(Provider entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into provicer (name,mobile_number,email,owner_info)
                    values (@name,@mobile_number,@email,@owner_info) RETURNING id",
                    new SqlParameters                    
                    {               
                        {"name", entity.Name},                    
                        {"mobile_number", entity.MobileNumber},    
                        {"email", entity.Email}, 
                        {"owner_info", entity.OwnerInfo}  
                    });

            return entity.Id;
        }

        public override bool Update(Provider entity)
        {
            var res = base.ExecuteNonQuery(
            @"update provicer set name=@name,mobile_number=@mobile_number,email=@email,owner_info=@owner_info
                WHERE id=@id",
                new SqlParameters
                    {                         
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
            return base.ExecuteScalar<int>("select count(*) from provicer");
        }

        public Provider GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                "select * from provicer where id = @id",
                new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from provicer where id = @id",
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

        public IList<Provider> GetAll()
        {
            return base.ExecuteSelect("select * from provicer");
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
