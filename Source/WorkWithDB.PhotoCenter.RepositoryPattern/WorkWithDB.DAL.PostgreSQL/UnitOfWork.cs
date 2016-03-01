using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using Npgsql;
using WorkWithDB.DAL.PostgreSQL.Repository;

namespace WorkWithDB.DAL.PostgreSQL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NpgsqlConnection _connection;
        private readonly NpgsqlTransaction _transaction;

        private IGoodsRepository _goodsRepository;

        public UnitOfWork(string ip) : this(ip, "5432", "photocenterDB", "postgres", "1")
        {
        }

        public UnitOfWork() : this("192.168.0.104", "5432", "photocenterDB", "postgres", "1")
        {
        }

        public UnitOfWork(string ip, string port, string dataBaseName, string name, string password)
        {
            var connectionString = 
                string.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};", ip, port, dataBaseName, name, password);

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
