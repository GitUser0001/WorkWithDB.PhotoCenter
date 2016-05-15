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
    internal class AvailabilityRepository : BaseRepository<int, Availability>, IAvailabilityRepository
    {
        public AvailabilityRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override int Save(Availability entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into availability (goods_id,count,structural_unit_id) 
                        values (@goods_id,@count,@structural_unit_id) RETURNING id",
                    new SqlParameters                    
                    {               
                        {"goods_id", entity.Goods.Id},                    
                        {"count", entity.Count},                    
                        {"structural_unit_id", entity.SrtucturalUnit.Id}                    
                    });

            return entity.Id;
        }

        public override bool Update(Availability entity)
        {
            var res = base.ExecuteNonQuery(
                        @"update availability set goods_id = @goods_id, count = @count, structural_unit_id = @structural_unit_id
                            WHERE id = @id;",
                        new SqlParameters
                        {
                            {"id", entity.Id},
                            {"goods_id", entity.Goods.Id},                    
                            {"count", entity.Count},                    
                            {"structural_unit_id", entity.SrtucturalUnit.Id}  
                        });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from availability");
        }

        public Availability GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from availability where id = @id",
                    new SqlParameters()                        
                    {
                        {"id", id}
                    });
        }

        public IList<Availability> GetByGoodsID(int goodsId)
        {
            return base.ExecuteSelect(
                    "select * from availability where goods_id = @id",
                    new SqlParameters()                        
                    {
                        {"id", goodsId}
                    });
        }

        public IList<Availability> GetBySrtUnitID(int strUnitId)
        {
            return base.ExecuteSelect(
                    "select * from availability where structural_unit_id = @id",
                    new SqlParameters()                        
                    {
                        {"id", strUnitId}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from availability where id = @id",
                        new SqlParameters()
                        {
                            {"id", id}
                        });

            if(res > 1)
            {
                throw new InvalidOperationException("Multiple rows deleted by single delete query");
            }

            return res == 1;
        }


        public IList<Availability> GetAll()
        {
            return base.ExecuteSelect("select * from availability");
        }

        protected override Availability DefaultRowMapping(Npgsql.NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                 return new Availability                            
                 {
                     Id = (int)reader["id"],
                     Goods = unitOfWork.GoodsRepository.GetByID((int)reader["goods_id"]),                                
                     Count = (int)reader["count"],                                
                     SrtucturalUnit = unitOfWork.
                                        StructuralUnitRepository.GetByID((int)reader["structural_unit_id"])                            
                 };
            }            
        }
    }
}
