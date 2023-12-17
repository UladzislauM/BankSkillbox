using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Bank
{
    /// <summary>
    /// Class for connection to DB with the EntityFramework method.
    /// </summary>
    internal class ConnectionDBMSQL
    {
        private DbMyEntitiesContext _connectionContext;

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
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DBConnectionMS"].ConnectionString);

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
