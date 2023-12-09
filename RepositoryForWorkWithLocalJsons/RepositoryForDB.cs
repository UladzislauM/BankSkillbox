using MarshalsExceptions;
using Newtonsoft.Json;
using RepositoryForWorkWithLocalJsons;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

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

                using (SqlConnection connection = _connection.GetConnection())
                {
                    string sql = "INSERT INTO [dbo].[clients] ([id], [first_name], [last_name], [history], [prestige], [status]) " +
                        "VALUES (@Id, @FirstName, @LastName, @History, @Prestige, @Status)";

                    using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                    {
                        ClientPrepareToSaveToDB(client, command).ExecuteNonQuery();
                    }
                    _connection.Dispose();
                }
                return client;
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

                using (SqlConnection connection = _connection.GetConnection())
                {

                    string sql = "UPDATE [dbo].[clients] " +
                                 "SET [first_name] = @FirstName, " +
                                     "[last_name] = @LastName, " +
                                     "[history] = @History, " +
                                     "[prestige] = @Prestige, " +
                                     "[status] = @Status " +
                                 "WHERE [id] = @Id";

                    using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                    {
                        ClientPrepareToSaveToDB(client, command).ExecuteNonQuery();
                    }
                    _connection.Dispose();
                }
                return client;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        private SqlCommand ClientPrepareToSaveToDB(Client client, SqlCommand command)
        {
            command.Parameters.AddWithValue("@Id", client.Id);
            command.Parameters.AddWithValue("@FirstName", client.FirstName ?? "newClientsName");
            command.Parameters.AddWithValue("@LastName", client.LastName ?? "newClientsLName");
            command.Parameters.AddWithValue("@History", client.History ?? "newHistory");
            command.Parameters.AddWithValue("@Prestige", client.Prestige);
            command.Parameters.AddWithValue("@Status", client.Status.ToString());

            return command;
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

                using (SqlConnection connection = _connection.GetConnection())
                {
                    string sql = "INSERT INTO [dbo].[scores] " +
                            "([id], [balance], [percent], [date_score], [is_capitalization], [is_money], [deadline], [date_last_dividends], [client_id], [score_type], [is_active]) " +
                            "VALUES (@Id, @Balance, @Percent, @DateScore, @IsCapitalization, @IsMoney, @Deadline, @DateLastDividends, @ClientId, @ScoreType, @IsActive)";

                    using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                    {
                        ScorePrepareToSaveToDB(score, command).ExecuteNonQuery();
                    }
                    _connection.Dispose();
                }
                return score;
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

                using (SqlConnection connection = _connection.GetConnection())
                {

                    string sql = "UPDATE [dbo].[scores] " +
                                 "SET [balance] = @Balance, " +
                                     "[percent] = @Percent, " +
                                     "[date_score] = @DateScore, " +
                                     "[is_capitalization] = @IsCapitalization, " +
                                     "[is_money] = @IsMoney, " +
                                     "[deadline] = @Deadline, " +
                                     "[date_last_dividends] = @DateLastDividends, " +
                                     "[client_id] = @ClientId, " +
                                     "[score_type] = @ScoreType, " +
                                     "[is_active] = @IsActive " +
                                 "WHERE [id] = @Id";

                    using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                    {
                        ScorePrepareToSaveToDB(score, command).ExecuteNonQuery();
                    }
                    _connection.Dispose();
                }
                return score;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        private SqlCommand ScorePrepareToSaveToDB(Score score, SqlCommand command)
        {
            command.Parameters.AddWithValue("@Id", score.Id);
            command.Parameters.AddWithValue("@Balance", score.Balance);
            command.Parameters.AddWithValue("@Percent", score.Percent);
            command.Parameters.AddWithValue("@DateScore", score.DateScore);
            command.Parameters.AddWithValue("@IsCapitalization", score.IsСapitalization ? 1 : 0);
            command.Parameters.AddWithValue("@IsMoney", score.IsMoney ? 1 : 0);
            command.Parameters.AddWithValue("@Deadline", score.Deadline);
            command.Parameters.AddWithValue("@DateLastDividends", score.DateLastDividends);
            command.Parameters.AddWithValue("@ClientId", score.Client.Id);
            command.Parameters.AddWithValue("@ScoreType", score.ScoreType);
            command.Parameters.AddWithValue("@IsActive", score.IsActive ? 1 : 0);

            return command;
        }

        /// <summary>
        /// Save all entities data to the DB
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="scores"></param>
        /// <exception cref="Exception"></exception>
        public void SaveAllDataToDB(List<Client> clients, List<Score> scores)
        {
            try
            {
                _connection.Connect();

                using (SqlConnection connection = _connection.GetConnection())
                {
                    SqlCommand command;

                    StringBuilder sql = new StringBuilder(
                        "INSERT INTO [dbo].[clients] ([id], [first_name], [last_name], [history], [prestige], [status]) " +
                        "VALUES (@Id, @first_name, @last_name, @history, @prestige, @status)");

                    foreach (Client client in clients)
                    {
                        using (command = new SqlCommand(sql.ToString(), connection))
                        {
                            ClientPrepareToSaveToDB(client, command).ExecuteNonQuery();
                        }
                    }
                    sql.Clear();

                    sql.Append("INSERT INTO [dbo].[scores] " +
                        "([Id], [balance], [percent], [date_score], [is_capitalization], [is_money], [deadline], [date_last_dividends], [client_id], [score_type], [is_active]) VALUES" +
                        " (@Id, @Balance, @Percent, @DateScore, @IsCapitalization, @IsMoney, @Deadline, @DateLastDividends, @ClientId, @ScoreType, @IsActive)");

                    foreach (Score score in scores)
                    {
                        using (command = new SqlCommand(sql.ToString(), connection))
                        {
                            ScorePrepareToSaveToDB(score, command).ExecuteNonQuery();
                        }
                    }
                    sql.Clear();

                    _connection.Dispose();
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
                _connection.Connect();

                List<T> objects = new List<T>();

                using (SqlConnection connection = _connection.GetConnection())
                {
                    if (typeof(T) == typeof(Client))
                    {
                        var sql = @"SELECT id, first_name, last_name, history, prestige, status
                                    FROM [dbo].[clients]";

                        SqlCommand command = new SqlCommand(sql, connection);
                        SqlDataReader readData = command.ExecuteReader();

                        while (readData.Read())
                        {
                            Client client = ConvertClientFromDB(readData);

                            objects.Add(client as T);
                        }
                    }
                    else if (typeof(T) == typeof(Score))
                    {
                        var sql = "SELECT id, balance, \"percent\", date_score, is_capitalization," +
                            " is_money, deadline, date_last_dividends, client_id, is_active, score_type " +
                            "FROM [dbo].[scores]";

                        SqlCommand command = new SqlCommand(sql, connection);
                        SqlDataReader readData = command.ExecuteReader();

                        while (readData.Read())
                        {
                            Score score = ConvertScoreFromDB(readData);

                            objects.Add(score as T);
                        }
                    }

                    _connection.Dispose();
                }
                return objects;
            }
            catch
            {
                throw new DBException("Find in DB exception");
            }
        }

        private Score ConvertScoreFromDB(SqlDataReader readData)
        {
            Score score = new Score(
                (Score.ScoreTypes)Enum.Parse(typeof(Score.ScoreTypes), readData["score_type"].ToString()));
            score.Id = Convert.ToInt32(readData["id"]);
            score.Balance = Convert.ToDecimal(readData["balance"]);
            score.Percent = Convert.ToDecimal(readData["percent"]);
            score.DateScore = Convert.ToDateTime(readData["date_score"]);
            score.IsСapitalization = Convert.ToBoolean(readData["is_capitalization"]);
            score.IsMoney = Convert.ToBoolean(readData["is_money"]);
            score.Deadline = Convert.ToDateTime(readData["deadline"]);
            score.DateLastDividends = Convert.ToDateTime(readData["date_last_dividends"]);
            score.Client = (Client)FindEntityByIdFromDB<Client>(Convert.ToInt32(readData["client_id"]));
            score.IsActive = Convert.ToBoolean(readData["is_active"]);
            return score;
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

                using (SqlConnection connection = _connection.GetConnection())
                {
                    if (typeof(T) == typeof(Client))
                    {
                        var sql = @"SELECT id, first_name, last_name, history, prestige, status
                                    FROM [dbo].[clients]
                                    WHERE id = @Id";

                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@Id", id);

                        SqlDataReader readData = command.ExecuteReader();

                        if (readData.Read())
                        {
                            Client client = ConvertClientFromDB(readData);
                            resualt = client;
                        }
                    }
                    else if (typeof(T) == typeof(Score))
                    {
                        var sql = "SELECT id, balance, \"percent\", date_score, is_capitalization," +
                            " is_money, deadline, date_last_dividends, client_id, is_active, score_type " +
                                   "FROM [dbo].[scores]" +
                                   "WHERE id = @id";

                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@Id", id);

                        SqlDataReader readData = command.ExecuteReader();

                        while (readData.Read())
                        {
                            Score score = ConvertScoreFromDB(readData);
                            resualt = score;
                        }
                    }
                    _connection.Dispose();
                }
                return resualt as T;
            }
            catch
            {
                throw new DBException("Find in DB exception");
            }
        }

        private Client ConvertClientFromDB(SqlDataReader readData)
        {
            Client client = new Client(
                (Client.Statuses)Enum.Parse(typeof(Client.Statuses), readData["status"].ToString()));
            client.Id = Convert.ToInt32(readData["id"]);
            client.FirstName = readData["first_name"].ToString();
            client.LastName = readData["last_name"].ToString();
            client.History = readData["history"].ToString();
            client.Prestige = Convert.ToInt32(readData["prestige"]);
            return client;
        }

    }
}