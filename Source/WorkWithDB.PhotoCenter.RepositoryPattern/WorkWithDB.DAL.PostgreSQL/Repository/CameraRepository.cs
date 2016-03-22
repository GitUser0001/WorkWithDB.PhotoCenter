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
    internal class CameraRepository : BaseRepository<int, Camera>, ICameraRepository
    {
        private readonly string tabelName = "cameras";

        public CameraRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }


        public override int Save(Camera entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,date_of_issue,resolution,model,cost_hour,firm,made_in)
                    values (@id,@date_of_issue,@resolution,@model,@cost_hour,@firm,@made_in) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},
                        {"date_of_issue", entity.DateOfIssue},
                        {"resolution", entity.Resolution},
                        {"model", entity.Model},
                        {"cost_hour", entity.CostHour},
                        {"firm", entity.Firm},
                        {"made_in", entity.MadeIn}
                    });
        }

        public override bool Update(Camera entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set service_id = id=@id,date_of_issue=@date_of_issue,
                resolution=@resolution,model=@model,cost_hour=@cost_hour,firm=@firm,made_in=@made_in",
                new SqlParameters
                        {
                            {"tableName", tabelName},
                            {"id", entity.Id},
                            {"date_of_issue", entity.DateOfIssue},
                            {"resolution", entity.Resolution},
                            {"model", entity.Model},
                            {"cost_hour", entity.CostHour},
                            {"firm", entity.Firm},
                            {"made_in", entity.MadeIn}   
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

        public Camera GetByID(int id)
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

        public IList<Camera> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override Camera DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new Camera
            {
                Id = (int)reader["id"],
                CostHour = (float)reader["cost_hour"],
                DateOfIssue = (DateTime)reader["data_of_issue"],
                Firm = (string)reader["firm"],
                MadeIn = (string)reader["made_id"],
                Model = (string)reader["model"],
                Resolution = (string)reader["resolution"]                                               
            };
        }
    }
}
