using System;
using System.Data.SqlClient;

namespace RepositoryForWorkWithLocalJsons
{
    /// <summary>
    /// Class for connection to DB with the ADO.net method.
    /// </summary>
    internal class ConnectionDBMSQL : IDisposable
    {
        private string DBSource;
        private string DBName;

        private SqlConnection _connection;

        public ConnectionDBMSQL(string dBSource, string dBName)
        {
            DBSource = dBSource;
            DBName = dBName;
        }

        /// <summary>
        /// Connect to db ADO.net.
        /// </summary>
        /// <param name="dBName"></param>
        /// <param name="dBSource"></param>
        public void Connect()
        {
            try
            {
                SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = DBSource,
                    InitialCatalog = DBName,
                    IntegratedSecurity = true,
                    Pooling = true
                };

                _connection = new SqlConnection(sqlConnectionBuilder.ConnectionString);
                _connection.Open();
            }
            catch
            {
                _connection = null;
            }
        }

        /// <summary>
        /// Get db ADO.net connection.
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            return _connection;
        }

        /// <summary>
        /// Dispose db ADO.net connection.
        /// </summary>
        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }
    }
}
