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
    internal class KioskRepository : BaseRepository<int, Kiosk>, IKioskRepository
    {
        public KioskRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }        

        public override int Save(Kiosk entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into kiosk (structure_unit_id, filiya_id)
                    values (@structure_unit_id,@filiya_id) RETURNING id",
                    new SqlParameters                    
                    {
                        {"structure_unit_id", entity.StructureUnit.Id},                    
                        {"filiya_id", entity.Filiya.StructureUnit.Id},                                    
                    });

            return entity.Id;
        }

        public override bool Update(Kiosk entity)
        {
            var res = base.ExecuteNonQuery(
            @"update kiosk set structure_unit_id=@structure_unit_id,filiya_id=@filiya_id
                WHERE id=@id",
                new SqlParameters
                    {
                        {"id", entity.Id},
                        {"structure_unit_id", entity.StructureUnit.Id},                    
                        {"filiya_id", entity.Filiya.StructureUnit.Id},    
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from kiosk");
        }

        public Kiosk GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from kiosk where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public Kiosk GetByStructUnitID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from kiosk where structure_unit_id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from kiosk where id = @id",
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

        public IList<Kiosk> GetAll()
        {
            return base.ExecuteSelect("select * from kiosk");
        }

        protected override Kiosk DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new Kiosk
                {
                    Id = (int)reader["id"],
                    StructureUnit = unitOfWork.StructuralUnitRepository.GetByID((int)reader["structure_unit_id"]),
                    Filiya = unitOfWork.FiliyaRepository.GetByID((int)reader["filiya_id"])
                };
            }
        }
    }
}
