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
    internal class ServiceRepository : BaseRepository<int, Service>, IServiceRepository
    {
        public ServiceRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(Service entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into service (order_id,type)
                    values (@order_id,@type) RETURNING id",
                    new SqlParameters                    
                    {                      
                        {"order_id", entity.Order.Id},                    
                        {"type", entity.Type}                   
                    });

            return entity.Id;
        }

        public override bool Update(Service entity)
        {
            var res = base.ExecuteNonQuery(
            @"update service set order_id=@order_id,type=@type
                WHERE id=@id",
                new SqlParameters
                    {                         
                        {"id", entity.Id},                    
                        {"order_id", entity.Order.Id},                    
                        {"type", entity.Type}   
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from service");
        }

        public Service GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from service where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from service where id = @id",
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

        public IList<Service> GetAll()
        {
            return base.ExecuteSelect("select * from service");
        }

        protected override Service DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new Service
                {
                    Id = (int)reader["id"],
                    Order = unitOfWork.OrderRepository.GetByID((int)reader["order_id"]),
                    Type = unitOfWork.ServiceTypeRepository.GetByID((int)reader["type"]),
                };
            }
        }
    }
}
