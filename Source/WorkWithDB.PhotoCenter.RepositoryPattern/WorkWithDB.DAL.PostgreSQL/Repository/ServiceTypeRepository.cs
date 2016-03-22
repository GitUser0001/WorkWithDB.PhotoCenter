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
    internal class ServiceTypeRepository : BaseRepository<int, ServiceType>, IServiceTypeRepository
    {
        private readonly string tabelName = "service_type";

        public ServiceTypeRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(ServiceType entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id, service_name)
                    values (@id, @service_name) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"service_name", entity.Name}                 
                    });
        }

        public override bool Update(ServiceType entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id, service_name=@service_name",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"service_name", entity.Name}     
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

        public ServiceType GetByID(int id)
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

        public IList<ServiceType> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override ServiceType DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new ServiceType
            {
                Id = (int)reader["id"],
                Name = (string)reader["service_name"]                
            };
        }
    }
}
