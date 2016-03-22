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
    internal class StructuralUnitRepository : BaseRepository<int, StructuralUnit>, IStructuralUnitRepository
    {
        private readonly string tabelName = "structural_unit";

        public StructuralUnitRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override int Save(StructuralUnit entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,name,owner_info,adress,opening_date,jobs)
                    values (@id,@name,@owner_info,@adress,@opening_date,@jobs) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"owner_info", entity.OwnerInfo},    
                        {"adress", entity.Adress},                    
                        {"opening_date", entity.Opening_Date},                    
                        {"jobs", entity.Jobs}                
                    });
        }

        public override bool Update(StructuralUnit entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,name=@name,owner_info=@owner_info,adress=@adress,opening_date=@opening_date,jobs=@jobs",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"owner_info", entity.OwnerInfo},    
                        {"adress", entity.Adress},                    
                        {"opening_date", entity.Opening_Date},                    
                        {"jobs", entity.Jobs}    
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

        public StructuralUnit GetByID(int id)
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

        public IList<StructuralUnit> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override StructuralUnit DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new StructuralUnit
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                OwnerInfo = (string)reader["owner_info"],
                Adress = (string)reader["adress"],
                Opening_Date = (DateTime)reader["opening_date"],
                Jobs = (int)reader["jobs"]
            };
        }
    }
}
