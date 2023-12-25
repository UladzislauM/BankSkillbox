using Bank;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Repositories
{
    /// <summary>
    /// Class for connection to DB with the EntityFramework method.
    /// </summary>
    internal class ConnectionToDb
    {
        private DbMyEntitiesContext _connectionContext;

        /// <summary>
        /// Connect to db ADO.net.
        /// </summary>
        public void Connect(string connectionName)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DbMyEntitiesContext>();

                if (connectionName == "DBConnectionMS")
                {
                    optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
                }
                else if (connectionName == "DBConnectionPSQL")
                {
                    optionsBuilder.UseNpgsql(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
                }
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
