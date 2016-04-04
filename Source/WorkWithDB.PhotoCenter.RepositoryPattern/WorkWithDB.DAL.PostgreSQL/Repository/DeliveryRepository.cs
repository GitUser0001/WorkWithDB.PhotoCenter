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
    internal class DeliveryRepository : BaseRepository<int, Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(Delivery entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into delivery (date_order,date_import,structural_unit_id,provider_id)
                    values (@date_order,@date_import,@structural_unit_id,@provider_id) RETURNING id",
                    new SqlParameters                    
                    {                   
                        {"date_order", entity.Date_order},    
                        {"date_import", entity.Date_import},
                        {"structural_unit_id", entity.StructuralUnit.Id},    
                        {"provider_id", entity.Provider.Id}                                   
                    });

            return entity.Id;
        }

        public override bool Update(Delivery entity)
        {
            var res = base.ExecuteNonQuery(
            @"update delivery set date_order=@date_order,date_import=@date_import,
                                    structural_unit_id=@structural_unit_id,provider_id=@provider_id
                            WHERE id=@id",
                new SqlParameters
                     {                         
                        {"id", entity.Id},                    
                        {"date_order", entity.Date_import},                    
                        {"structural_unit_id", entity.StructuralUnit.Id},    
                        {"provider_id", entity.Provider.Id}      
                     });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from delivery");
        }

        public Delivery GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from delivery where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from delivery where id = @id",
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

        public IList<Delivery> GetAll()
        {
            return base.ExecuteSelect("select * from delivery");
        }

        protected override Delivery DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new Delivery
                {
                    Id = (int)reader["id"],
                    Date_import = (DateTime)reader["date_import"],
                    Date_order = (DateTime)reader["date_order"],
                    Provider = unitOfWork.ProviderRepository.GetByID((int)reader["provider_id"]),
                    StructuralUnit = unitOfWork.StructuralUnitRepository.GetByID((int)reader["structural_unit_id"])
                };
            }
        }
    }
}
