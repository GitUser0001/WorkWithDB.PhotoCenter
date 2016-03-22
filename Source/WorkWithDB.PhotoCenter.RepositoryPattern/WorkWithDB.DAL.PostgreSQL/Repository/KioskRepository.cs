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
        private readonly string tabelName = "kiosk";

        public KioskRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override int Save(Kiosk entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (structure_unit_id, filiya_id)
                    values (@structure_unit_id,@filiya_id) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"structure_unit_id", entity.StructureUnitID},                    
                        {"filiya_id", entity.Filiya.StructureUnitID},                                    
                    });
        }

        public override bool Update(Kiosk entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set structure_unit_id=@structure_unit_id,filiya_id=@filiya_id",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"structure_unit_id", entity.StructureUnitID},                    
                        {"filiya_id", entity.Filiya.StructureUnitID},    
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

        public Kiosk GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from @tableName where structure_unit_id = @id",
                    new SqlParameters()                        
                    {
                        {"tableName", tabelName},
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from @tableName where structure_unit_id = @id",
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

        public IList<Kiosk> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override Kiosk DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new Kiosk
                {
                    StructureUnitID = (int)reader["structure_unit_id"],
                    Filiya = unitOfWork.FiliyaRepository.GetByID((int)reader["filiya_id"])
                };
            }
        }
    }
}
