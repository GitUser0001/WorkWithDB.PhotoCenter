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
    internal class PrintingRepository : BaseRepository<int, Printing>, IPrintingRepository
    {
        private readonly string tabelName = "printing";

        public PrintingRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(Printing entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (service_id,price,copy_count,discount,is_immediately,photo_format_id,paper_format_id)
                    values (@service_id,@price,@copy_count,@discount,@is_immediately,@photo_format_id,@paper_format_id) 
                    SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"service_id", entity.ServiceID},                    
                        {"price", entity.Price},                    
                        {"copy_count", entity.CopyCount},    
                        {"discount", entity.Discount},                    
                        {"is_immediately", entity.IsImmediately},                    
                        {"photo_format_id", entity.PhotoFormat.Id},
                        {"paper_format_id", entity.PaperFormat.Id}                 
                    });
        }

        public override bool Update(Printing entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set service_id=@service_id,price=@price,copy_count=@copy_count,discount=@discount,
                    is_immediately=@is_immediately,photo_format_id=@photo_format_id,paper_format_id=@paper_format_id",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"service_id", entity.ServiceID},                    
                        {"price", entity.Price},                    
                        {"copy_count", entity.CopyCount},    
                        {"discount", entity.Discount},                    
                        {"is_immediately", entity.IsImmediately},                    
                        {"photo_format_id", entity.PhotoFormat.Id},
                        {"paper_format_id", entity.PaperFormat.Id}    
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

        public Printing GetByID(int id)
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

        public IList<Printing> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override Printing DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new Printing
                {
                    ServiceID = (int)reader["service_id"],
                    PaperFormat = unitOfWork.PaperFormatRepository.GetByID((int)reader["paper_format_id"]),
                    PhotoFormat = unitOfWork.PhotoFormatRepository.GetByID((int)reader["photo_format_id"]),
                    CopyCount = (int)reader["copy_count"],
                    Discount = (int)reader["discount"],
                    IsImmediately = (bool)reader["is_immediately"],                    
                    Price = (float)reader["price"]
                };
            }
        }
    }
}
