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

        public Tkey SaveOrUpdate(TEntity entity)
        {
            if (Object.Equals(entity.Id, default(Tkey)))
            {
                return Insert(entity);
            }
            else
            {
                if (Update(entity))
                {
                    return entity.Id;
                }
                else
                {
                    return default(Tkey);
                }
            }
        }

        public abstract Tkey Insert(TEntity entity);
        public abstract bool Update(TEntity entity);

        /// <summary>
        /// Выполняет запрос и возвращает первый столбец первой строки результирующего набора, возвращаемого запросом. 
        /// Дополнительные столбцы и строки игнорируются.
        /// </summary>
        protected T ExecuteScalar<T>(string sql, IDictionary<string, object> parameters = null)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(sql, _connection, _transaction))
            {
                FillParameters(parameters, command);

                return (T)command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Используется для операций, где ничего не возвращаеться из SQL-запроса или хранимой процедуры. 
        /// Предпочтительным является использование будет для INSERT, UPDATE и DELETE операций.
        /// </summary>
        protected int ExecuteNonQuery(string sql, IDictionary<string, object> parameters = null)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(sql, _connection, _transaction))
            {
                FillParameters(parameters, command);

                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Выполняет запрос и возвращает первую строчку строки результирующего набора, возвращаемого запросом.
        /// </summary>
        protected T ExecuteSingleRowSelect<T>(
            string sql,
            Func<NpgsqlDataReader, T> rowMapping,
            IDictionary<string, object> parameters = null)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(sql, _connection, _transaction))
            {
                FillParameters(parameters, command);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return rowMapping(reader);
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
        }

        protected TEntity ExecuteSingleRowSelect(string sql, NpgsqlParameters sqlParameters = null)
        {
            return ExecuteSingleRowSelect(sql, DefaultRowMapping, sqlParameters);
        }

        /// <summary>
        /// Извлекает данные из основной базы данных, используя SQL-строку SelectCommand и параметры, 
        /// содержащиеся в коллекции SelectParameters.
        /// </summary>
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
