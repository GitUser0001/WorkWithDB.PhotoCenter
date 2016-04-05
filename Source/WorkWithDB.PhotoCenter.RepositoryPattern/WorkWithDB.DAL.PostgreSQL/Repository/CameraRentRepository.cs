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
    internal class CameraRentRepository : BaseRepository<int, CameraRent>, ICameraRentRepository
    {
        public CameraRentRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override int Save(CameraRent entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into camera_rent (service_id,filiya_id,price,passport_number,passport_code,camera_model,for_time)
                    values (@service_id,@filiya_id,@price,@passport_number,@passport_code,@camera_model,@for_time) RETURNING id",
                    new SqlParameters                    
                    {    
                        {"service_id", entity.Service.Id},                    
                        {"filiya_id", entity.Filiya.StructureUnit.Id},                    
                        {"price", entity.Price},    
                        {"passport_number", entity.PassportNumber},                    
                        {"passport_code", entity.PassportCode},                    
                        {"camera_model", entity.Camera.Id},
                        {"for_time", entity.PeriodOfTime}                 
                    });

            return entity.Id;
        }

        public override bool Update(CameraRent entity)
        {
            var res = base.ExecuteNonQuery(
            @"update camera_rent set service_id = @service_id,filiya_id = @filiya_id,price = @price,
                passport_number = @passport_number,passport_code = @passport_code,camera_model = @camera_model,for_time = @for_time
                WHERE id=@id",
                new SqlParameters
                    {
                        {"id", entity.Id},
                        {"service_id", entity.Service.Id},                     
                        {"filiya_id", entity.Filiya.StructureUnit.Id},                    
                        {"price", entity.Price},    
                        {"passport_number", entity.PassportNumber},                    
                        {"passport_code", entity.PassportCode},                    
                        {"camera_model", entity.Camera.Id},
                        {"for_time", entity.PeriodOfTime}     
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from camera_rent");
        }

        public CameraRent GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from camera_rent where id = @id",
                    new SqlParameters()                        
                    {
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from camera_rent where id = @id",
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

        public IList<CameraRent> GetAll()
        {
            return base.ExecuteSelect("select * from camera_rent");
        }

        protected override CameraRent DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new CameraRent
                {
                    Id = (int)reader["id"],
                    Service = unitOfWork.ServiceRepository.GetByID((int)reader["service_id"]),
                    Filiya =  unitOfWork.FiliyaRepository.GetByID((int)reader["filiya_id"]),
                    Camera = unitOfWork.CameraRepository.GetByID((int)reader["camera_model"]),
                    PassportCode = (string)reader["passport_code"],
                    PassportNumber = (string)reader["passport_number"],
                    PeriodOfTime = (DateTime)reader["for_time"],
                    Price = (float)reader["price"]
                };
            }
        }
    }
}
