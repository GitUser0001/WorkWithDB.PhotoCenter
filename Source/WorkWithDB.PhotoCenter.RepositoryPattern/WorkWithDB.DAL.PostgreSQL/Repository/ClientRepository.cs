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
    internal class ClientRepository : BaseRepository<int, Client>, IClientRepository
    {
        private readonly string tabelName = "client";

        public ClientRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }


        public override int Save(Client entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,registration_date,card_id,full_name,is_profesional)
                    values (@id,@registration_date,@card_id,@full_name,@is_profesional) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},
                        {"registration_date", entity.RegistrationDate},                    
                        {"card_id", entity.DiscountCard.Id},                    
                        {"full_name", entity.FullName},    
                        {"is_profesional", entity.IsProfesional},                                   
                    });
        }

        public override bool Update(Client entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,registration_date=@registration_date,
                                card_id=@card_id,full_name=@full_name,is_profesional=@is_profesional",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.Id},
                        {"registration_date", entity.RegistrationDate},                    
                        {"card_id", entity.DiscountCard.Id},                    
                        {"full_name", entity.FullName},    
                        {"is_profesional", entity.IsProfesional},     
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

        public Client GetByID(int id)
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

        public IList<Client> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override Client DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new Client
                {
                    Id = (int)reader["id"],
                    DiscountCard = unitOfWork.DiscountCardRepository.GetByID((int)reader["card_id"]),
                    FullName = (string)reader["full_name"],
                    IsProfesional = (bool)reader["is_profesional"],
                    RegistrationDate = (DateTime)reader["registration_date"]
                };
            }
        }
    }
}
