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
    internal class FiliyaRepository : BaseRepository<int, Filiya>, IFiliyaRepository
    {
        private readonly string tabelName = "\"BD_LAB\".\"filiya\"";

        public FiliyaRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override int Save(Filiya entity)
        {
            return (int)
            base.ExecuteNonQuery(
                    "insert into \"BD_LAB\".\"filiya\" (\"id\",\"structural_unit_id\") values (@id, @stUnitID)",
                    new SqlParameters
                    {          
                        //{"tableName", tabelName},
                        {"stUnitID", entity.StructureUnitID},
                        {"id", entity.Id},                                   
                    });
            //return 1;
        }

        public override bool Update(Filiya entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.StructureUnitID},        
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

        public Filiya GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from @tableName where service_id = @id",
                    new SqlParameters()                        
                    {
                        {"tableName", tabelName},
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from @tableName where service_id = @id",
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

        public IList<Filiya> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }
        
        protected override Filiya DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new Filiya
            {
                Id = (int)reader["id"],
                StructureUnitID = (int)reader["structural_unit_id"]
            };
        }
    }
}
