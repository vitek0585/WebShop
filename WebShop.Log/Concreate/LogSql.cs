using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebShop.Log.Abstract;

namespace WebShop.Log.Concreate
{

    public sealed class LogSql : LogBase<String>
    {
        private Lazy<SqlConnection> _conection;
        private string _query;
        public LogSql()
        {
            _query = "insert into Log (message,ecxeptionType,stackTrace,httpMethod,path,urlReferrer,userAgent,isAuthenticated,type) " +
                                  "values (@message,@ecxeptionType,@stackTrace,@httpMethod,@path,@urlReferrer,@userAgent,@isAuthenticated,@type)";

            _conection = new Lazy<SqlConnection>(GetConnection);
        }
        public LogSql(string sqlInsert)
        {
            _query = sqlInsert;
            _conection = new Lazy<SqlConnection>(GetConnection);
        }
        private SqlConnection GetConnection()
        {
            var str = ConfigurationManager.ConnectionStrings["ShopContext"].ConnectionString;
            var sql = new SqlConnection(str);
            sql.Open();
            return sql;
        }

        protected override void Execute(TypeLog typeLog, string messageLog, Exception exception)
        {
            var command = new SqlCommand(_query, _conection.Value);
            var message = new SqlParameter("@message", SqlDbType.NVarChar) { Value = (object)messageLog ?? DBNull.Value };
            IncludeException(command, exception);
            var httpMethod = new SqlParameter("@httpMethod", SqlDbType.NVarChar) { Value = (object)HttpMethod ?? DBNull.Value };
            var path = new SqlParameter("@path", SqlDbType.NVarChar) { Value = (object)Path ?? DBNull.Value };
            var urlReferrer = new SqlParameter("@urlReferrer", SqlDbType.NVarChar) { Value = (object)UrlReferrer ?? DBNull.Value };
            var userAgent = new SqlParameter("@userAgent", SqlDbType.NVarChar) { Value = (object)UserAgent ?? DBNull.Value };
            var isAuthenticated = new SqlParameter("@isAuthenticated", SqlDbType.Bit) { Value = IsAuthenticated };

            var type = new SqlParameter("@type", SqlDbType.TinyInt) { Value = (int)typeLog };

            command.Parameters.AddRange(new[] { message, httpMethod, path, urlReferrer, userAgent, isAuthenticated, type });
            command.ExecuteNonQuery();
        }

        private void IncludeException(SqlCommand command, Exception exception)
        {
            SqlParameter exc = new SqlParameter("@ecxeptionType", SqlDbType.NVarChar);
            SqlParameter st = new SqlParameter("@stackTrace", SqlDbType.NVarChar);
            if (exception != null)
            {
                exc.Value = exception.GetType();
                st.Value = exception.StackTrace;
            }
            else
            {
                exc.Value = DBNull.Value;
                st.Value = DBNull.Value;
            }
            command.Parameters.AddRange(new[] { exc, st });
        }

        private bool _disposed;
        public override void Dispose()
        {
            if (!_disposed && _conection.IsValueCreated)
            {
                try
                {
                    _conection.Value.Dispose();
                    _conection.Value.Close();
                }
                catch (SqlException e)
                {
                    throw new Exception("Dispose LogSql", e);
                }
                _disposed = true;
                GC.SuppressFinalize(this);
            }
            
        }

        ~LogSql()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }

    }
}