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
    internal class PhotoServiceRepository : BaseRepository<int, PhotoService>, IPhotoServiceRepository
    {
        private readonly string tabelName = "photo_service";

        public PhotoServiceRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }


        public override int Save(PhotoService entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (service_id,photo_count,photographer_id,filiya_id,is_immediately,price)
                    values (@service_id,@photo_count,@photographer_id,@filiya_id,@is_immediately,@price) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"service_id", entity.ServiceID},                    
                        {"photo_count", entity.PhotoCount},                    
                        {"photographer_id", entity.Photographer.Id},    
                        {"filiya_id", entity.Filiya.StructureUnitID},                    
                        {"is_immediately", entity.IsImmediately},                    
                        {"price", entity.Price}            
                    });
        }

        public override bool Update(PhotoService entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set service_id=@service_id,photo_count=@photo_count,
                photographer_id=@photographer_id,filiya_id=@filiya_id,is_immediately=@is_immediately,price=@price",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"service_id", entity.ServiceID},                    
                        {"photo_count", entity.PhotoCount},                    
                        {"photographer_id", entity.Photographer.Id},    
                        {"filiya_id", entity.Filiya.StructureUnitID},                    
                        {"is_immediately", entity.IsImmediately},                    
                        {"price", entity.Price}       
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

        public PhotoService GetByID(int id)
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

        public IList<PhotoService> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override PhotoService DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new PhotoService
                {
                    ServiceID = (int)reader["service_id"],
                    Filiya = unitOfWork.FiliyaRepository.GetByID((int)reader["filiya_id"]),
                    Photographer = unitOfWork.PhotographerRepository.GetByID((int)reader["photographer_id"]),
                    PhotoCount = (int)reader["photo_count"],
                    IsImmediately = (bool)reader["is_immediately"],
                    Price = (float)reader["price"]
                };
            }
        }
    }
}
