using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Bank
{
    /// <summary>
    /// Class for connection to DB with the EntityFramework method.
    /// </summary>
    internal class ConnectionDBMSQL
    {
        private string DBSource;
        private string DBName;

        private DbMyEntitiesContext _connectionContext;

        public ConnectionDBMSQL(string dBSource, string dBName)
        {
            DBSource = dBSource;
            DBName = dBName;
        }

        public void UpdateConnection(string dBSource, string dBName)
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
                var optionsBuilder = new DbContextOptionsBuilder<DbMyEntitiesContext>();
                optionsBuilder.UseSqlServer(new SqlConnectionStringBuilder
                {
                    DataSource = DBSource,
                    InitialCatalog = DBName,
                    IntegratedSecurity = true,
                    Pooling = true
                }.ConnectionString);

                _connectionContext = new DbMyEntitiesContext(optionsBuilder.Options);
            }
            catch
            {
                _connectionContext = null;
            }
        }

        /// <summary>
        /// Get db ADO.net connection.
        /// </summary>
        /// <returns></returns>
        public DbMyEntitiesContext GetConnection()
        {
            return _connectionContext;
        }

    }
}
