using MarshalsExceptions;
using Microsoft.EntityFrameworkCore;

namespace Bank
{
    /// <summary>
    /// Repository for DB using.
    /// </summary>
    public class RepositoryForDB
    {
        private ConnectionDBMSQL _connection;

        public RepositoryForDB(string dBSource, string dBName)
        {
            _connection = new ConnectionDBMSQL(dBSource, dBName);
        }

        public void UpdateConnection(string dBSource, string dBName)
        {
            _connection.UpdateConnection(dBSource, dBName);
        }

        /// <summary>
        /// Saving the client to the DB.
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="Exception"></exception>
        public Client SaveClientToDB(Client client)
        {
            try
            {
                _connection.Connect();

                if (_connection.GetConnection() != null)
                {
                    using (DbMyEntitiesContext connection = _connection.GetConnection())
                    {
                        connection.Clients.Add(client);
                        connection.SaveChanges();
                    }
                }
                return client;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// Saving the score to the DB.
        /// </summary>
        /// <param name="score"></param>
        /// <exception cref="Exception"></exception>
        public Score SaveScoreToDB(Score score)
        {
            try
            {
                _connection.Connect();

                if (_connection.GetConnection() != null)
                {
                    using (DbMyEntitiesContext connection = _connection.GetConnection())
                    {
                        connection.Scores.Add(score);
                        connection.SaveChanges();
                    }
                }
                return score;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// The method for saving all entities data to the DB
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="scores"></param>
        /// <exception cref="Exception"></exception>
        public void SaveAllDataToDB(List<Client> clients, List<Score> scores)
        {
            try
            {
                _connection.Connect();

                if (_connection.GetConnection() != null)
                {
                    using (DbMyEntitiesContext connection = _connection.GetConnection())
                    {
                        foreach (var client in clients)
                        {
                            connection.Clients.Add(client);
                        }

                        foreach (var score in scores)
                        {
                            connection.Scores.Add(score);
                        }

                        connection.SaveChanges();
                    }
                }
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// Update client into the DB.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public Client UpdateClientIntoDB(Client client)
        {
            try
            {
                _connection.Connect();

                if (_connection.GetConnection() != null)
                {
                    using (DbMyEntitiesContext connection = _connection.GetConnection())
                    {
                        Client existingClient = connection.Clients.Find(client.Id);

                        if (existingClient != null)
                        {
                            existingClient.FirstName = client.FirstName;
                            existingClient.LastName = client.LastName;
                            existingClient.History = client.History;
                            existingClient.Prestige = client.Prestige;
                            existingClient.Status = client.Status;

                            connection.SaveChanges();
                        }

                        return client;
                    }
                }
                return client;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// Update score into the DB.
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public Score UpdateScoreIntoDB(Score score)
        {
            try
            {
                _connection.Connect();

                if (_connection.GetConnection() != null)
                {
                    using (DbMyEntitiesContext connection = _connection.GetConnection())
                    {
                        Score existingScore = connection.Scores.Include(s => s.Client)
                            .FirstOrDefault(s => s.Id == score.Id);

                        if (existingScore != null)
                        {
                            existingScore.Balance = score.Balance;
                            existingScore.Percent = score.Percent;
                            existingScore.DateScore = score.DateScore;
                            existingScore.IsCapitalization = score.IsCapitalization;
                            existingScore.IsMoney = score.IsMoney;
                            existingScore.Deadline = score.Deadline;
                            existingScore.DateLastDividends = score.DateLastDividends;
                            existingScore.Client.Id = score.Client.Id;
                            existingScore.ScoreType = score.ScoreType;
                            existingScore.IsActive = score.IsActive;

                            connection.SaveChanges();
                        }

                        return score;
                    }
                }
                return score;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// The method for updating all entities in the DB.
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="scores"></param>
        /// <exception cref="DBException"></exception>
        public void UpdateAllEntitiesIntoDB(List<Client> clients, List<Score> scores)
        {
            try
            {
                _connection.Connect();

                if (_connection.GetConnection() != null)
                {
                    using (DbMyEntitiesContext connection = _connection.GetConnection())
                    {
                        foreach (var client in clients)
                        {
                            var existingClient = connection.Clients.FirstOrDefault(c => c.Id == client.Id);

                            if (existingClient != null)
                            {
                                connection.Entry(existingClient).CurrentValues.SetValues(client);
                            }
                            else
                            {
                                connection.Clients.Add(client);
                            }
                        }

                        foreach (var score in scores)
                        {
                            var existingScore = connection.Scores.FirstOrDefault(s => s.Id == score.Id);

                            if (existingScore != null)
                            {
                                connection.Entry(existingScore).CurrentValues.SetValues(score);
                            }
                            else
                            {
                                connection.Scores.Add(score);
                            }
                        }

                        connection.SaveChanges();
                    }
                }
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        /// <summary>
        /// Load entity data from db (clients or scores)
        /// </summary>
        /// <typeparam name="T">Might be the client or score</typeparam>
        /// <returns>Might be the list of clients or scores</returns>
        /// <exception cref="Exception"></exception>
        public List<T> FindEntitiesDataFromDB<T>() where T : class
        {
            try
            {
                List<T> objects = new List<T>();

                _connection.Connect();

                if (_connection.GetConnection() != null)
                {
                    using (DbMyEntitiesContext connection = _connection.GetConnection())
                    {
                        if (typeof(T) == typeof(Client))
                        {
                            return connection.Clients.OfType<T>().ToList();
                        }
                        else if (typeof(T) == typeof(Score))
                        {
                            return connection.Scores.Include(c => c.Client).OfType<T>().ToList();
                        }
                    }
                }
                return objects;
            }
            catch
            {
                throw new DBException("Find in DB exception");
            }
        }

        /// <summary>
        /// Load entity by id from the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public T FindEntityByIdFromDB<T>(long id) where T : class
        {
            try
            {
                _connection.Connect();
                object resualt = null;

                if (_connection.GetConnection() != null)
                {

                    using (DbMyEntitiesContext connection = _connection.GetConnection())
                    {
                        if (typeof(T) == typeof(Client))
                        {
                            resualt = connection.Clients
                                .Where(c => c.Id == id)
                                .FirstOrDefault();
                        }
                        else if (typeof(T) == typeof(Score))
                        {
                            resualt = connection.Scores
                                 .Where(s => s.Id == id)
                                 .FirstOrDefault();
                        }
                    }
                }
                return resualt as T;
            }
            catch
            {
                throw new DBException("Find in DB exception");
            }
        }

    }
}