﻿using Npgsql;
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
        private readonly string tabelName = "delivery";

        public DeliveryRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(Delivery entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id, date_order,date_import,structural_unit_id,provider_id)
                    values (@id,@date_order,@date_import,@structural_unit_id,@provider_id) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"date_order", entity.Date_order},    
                        {"date_import", entity.Date_import},
                        {"structural_unit_id", entity.StructuralUnit.Id},    
                        {"provider_id", entity.Provider.Id}                                   
                    });
        }

        public override bool Update(Delivery entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,date_order=@date_order,date_import=@date_import,
                                    structural_unit_id=@structural_unit_id,provider_id=@provider_id",
                new SqlParameters
                     {
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"date_order", entity.Date_import},                    
                        {"structural_unit_id", entity.StructuralUnit.Id},    
                        {"provider_id", entity.Provider.Id}      
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

        public Delivery GetByID(int id)
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

        public IList<Delivery> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
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
