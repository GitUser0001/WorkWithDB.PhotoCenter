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
    internal class OrderRepository : BaseRepository<int, Order>, IOrderRepository
    {
        public OrderRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(Order entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into order (structure_unit, client_id, sold_date)
                    values (@structure_unit, @client_id, @sold_date) RETURNING id",
                    new SqlParameters                    
                    {                   
                        {"structure_unit", entity.StructureUnit.Id},                    
                        {"client_id", entity.Client.Id},    
                        {"sold_date", entity.SoldDate}         
                    });

            return entity.Id;
        }

        public override bool Update(Order entity)
        {
            var res = base.ExecuteNonQuery(
            @"update order set structure_unit=@structure_unit, client_id=@client_id, sold_date=@sold_date
                WHERE id=@id",
                new SqlParameters
                    {                         
                        {"id", entity.Id},                    
                        {"structure_unit", entity.StructureUnit.Id},                    
                        {"client_id", entity.Client.Id},    
                        {"sold_date", entity.SoldDate}     
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from order");
        }

        public Order GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from order where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from order where id = @id",
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

        public IList<Order> GetAll()
        {
            return base.ExecuteSelect("select * from order");
        }

        protected override Order DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new Order
                {
                    Id = (int)reader["id"],
                    StructureUnit = unitOfWork.StructuralUnitRepository.GetByID((int)reader["structure_unit_id"]),
                    Client = unitOfWork.ClientRepository.GetByID((int)reader["client_id"]),
                    SoldDate = (DateTime)reader["sold_date"]
                };
            }
        }
    }
}
