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
        public ServiceTypeRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(ServiceType entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into service_type (service_name)
                    values (@service_name) RETURNING id",
                    new SqlParameters                    
                    {                  
                        {"service_name", entity.Name}                 
                    });

            return entity.Id;
        }

        public override bool Update(ServiceType entity)
        {
            var res = base.ExecuteNonQuery(
            @"update service_type set service_name=@service_name
                WHERE id=@id",
                new SqlParameters
                    {                         
                        {"id", entity.Id},                    
                        {"service_name", entity.Name}     
                    });

            return res > 0;
        }        

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from service_type");
        }

        public ServiceType GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from service_type where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from service_type where id = @id",
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

        public IList<ServiceType> GetAll()
        {
            return base.ExecuteSelect("select * from service_type");
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
