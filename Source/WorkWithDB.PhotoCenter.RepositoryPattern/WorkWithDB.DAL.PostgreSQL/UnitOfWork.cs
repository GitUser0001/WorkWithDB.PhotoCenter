using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Abstract.Repository;
using Npgsql;
using WorkWithDB.DAL.PostgreSQL.Repository;
using System.Configuration;

namespace WorkWithDB.DAL.PostgreSQL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NpgsqlConnection _connection;
        private readonly NpgsqlTransaction _transaction;

        private IGoodsRepository _goodsRepository;

        public UnitOfWork()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Home"].ConnectionString;

            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public IGoodsRepository GoodsRepository
        {
            get
            {
                if (_goodsRepository == null)
                {
                    _goodsRepository = new GoodsRepository(_connection, _transaction);
                }

                return _goodsRepository;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
            }
            finally
            {
                _connection.Dispose();
            }
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void RollBack()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }
    }
}
