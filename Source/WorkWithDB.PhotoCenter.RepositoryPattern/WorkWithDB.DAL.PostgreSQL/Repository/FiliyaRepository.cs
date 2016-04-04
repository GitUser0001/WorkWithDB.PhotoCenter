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
        private readonly string tabelName = "filiya";

        public FiliyaRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override int Save(Filiya entity)
        {
            entity.Id =
            base.ExecuteScalar<int>(
                    "insert into filiya (structural_unit_id) values (@stUnitID) RETURNING id",
                    new SqlParameters
                    {
                        {"stUnitID", entity.StructureUnitID},                               
                    });

            return entity.Id;
        }

        public override bool Update(Filiya entity)
        {
            var res = base.ExecuteNonQuery(
            @"update filiya set structural_unit_id=@st_unit_id
                WHERE id=@id",
                new SqlParameters
                    {
                        {"id", entity.Id},
                        {"st_unit_id", entity.StructureUnitID},        
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from filiya");
        }

        public Filiya GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from filiya where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from filiya where id = @id",
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

        public IList<Filiya> GetAll()
        {
            return base.ExecuteSelect("select * from filiya");
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
