using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;
using Npgsql;

namespace WorkWithDB.DAL.PostgreSQL.Infrastructure
{
    internal abstract class BaseRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        private readonly NpgsqlConnection _connection;
        private readonly NpgsqlTransaction _transaction;

        protected BaseRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        protected IList<T> ExecuteSelect<T>(
            string sql,
            Func<NpgsqlDataReader, T> rowMapping,
            IDictionary<string, object> parameters = null)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(sql, _connection, _transaction))
            {
                FillParameters(parameters, command);

                using (var reader = command.ExecuteReader())
                {
                    List<T> list = new List<T>(1);
                    while (reader.Read())
                    {
                        list.Add(rowMapping(reader));
                    }

                    return list;
                }
            }
        }

        protected IList<TEntity> ExecuteSelect(string sql, NpgsqlParameters sqlParameters = null)
        {
            return ExecuteSelect(sql, DefaultRowMapping, sqlParameters);
        }

        protected abstract TEntity DefaultRowMapping(NpgsqlDataReader reader);

        private static void FillParameters(IDictionary<string, object> parameters, NpgsqlCommand command)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                }
            }
        }
    }
}
