using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using MarshalsExceptions;

namespace Bank
{
    public class Repository
    {
        /// <summary>
        /// Saving the client to the DB.
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="Exception"></exception>
        public Client SaveClientToDB(Client client)
        {
            try
            {
                SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "bankdb",
                    IntegratedSecurity = true,
                    Pooling = true
                };

                using (SqlConnection connection = new SqlConnection(sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO [dbo].[clients] ([id], [first_name], [last_name], [history], [prestige], [status]) " +
                        "VALUES (@Id, @FirstName, @LastName, @History, @Prestige, @Status)";

                    using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                    {
                        ClientPrepareToSaveToDB(client, command).ExecuteNonQuery();
                    }
                    connection.Close();
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
                SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "bankdb",
                    IntegratedSecurity = true,
                    Pooling = true
                };

                using (SqlConnection connection = new SqlConnection(sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();

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
                    connection.Close();
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
                SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "bankdb",
                    IntegratedSecurity = true,
                    Pooling = true
                };

                using (SqlConnection connection = new SqlConnection(sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO [dbo].[scores] " +
                            "([id], [balance], [percent], [date_score], [is_capitalization], [is_money], [deadline], [date_last_dividends], [client_id], [score_type], [is_active]) " +
                            "VALUES (@Id, @Balance, @Percent, @DateScore, @IsCapitalization, @IsMoney, @Deadline, @DateLastDividends, @ClientId, @ScoreType, @IsActive)";

                    using (SqlCommand command = new SqlCommand(sql.ToString(), connection))
                    {
                        ScorePrepareToSaveToDB(score, command).ExecuteNonQuery();
                    }
                    connection.Close();
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
                SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "bankdb",
                    IntegratedSecurity = true,
                    Pooling = true
                };

                using (SqlConnection connection = new SqlConnection(sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();

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
                    connection.Close();
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
                SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "bankdb",
                    IntegratedSecurity = true,
                    Pooling = true
                };

                using (SqlConnection connection = new SqlConnection(sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
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

                    connection.Close();
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
        public List<T> LoadEntityDataFromDB<T>() where T : class
        {
            try
            {
                SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = @"(localdb)\MSSQLLocalDB",
                    InitialCatalog = "bankdb",
                    IntegratedSecurity = true,
                    Pooling = true
                };

                List<T> objects = new List<T>();
                using (SqlConnection connection = new SqlConnection(sqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    if (typeof(T) == typeof(Client))
                    {
                        var sql = @"SELECT id, first_name, last_name, history, prestige, status
                                    FROM [dbo].[clients]";

                        SqlCommand command = new SqlCommand(sql, connection);
                        SqlDataReader readData = command.ExecuteReader();

                        while (readData.Read())
                        {
                            Client client = ConvertClientFromDB(readData, false);

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

                    connection.Close();
                }
                return objects;
            }
            catch
            {
                throw new DBException("Save to DB exception");
            }
        }

        private static Score ConvertScoreFromDB(SqlDataReader readData)
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
            score.Client.Id = Convert.ToInt32(readData["client_id"]);
            score.IsActive = Convert.ToBoolean(readData["is_active"]);
            score.Client = ConvertClientFromDB(readData, true);
            return score;
        }

        private static Client ConvertClientFromDB(SqlDataReader readData, bool isScore)
        {
            string prefix = "";
            if (isScore)
            {
                prefix = "score.";
            }

            Client client = new Client(
                (Client.Statuses)Enum.Parse(typeof(Client.Statuses), readData[prefix + "status"].ToString()));
            client.Id = Convert.ToInt32(readData[prefix + "id"]);
            client.FirstName = readData[prefix + "first_name"].ToString();
            client.LastName = readData[prefix + "last_name"].ToString();
            client.History = readData[prefix + "history"].ToString();
            client.Prestige = Convert.ToInt32(readData[prefix + "prestige"]);
            return client;
        }

        /// <summary>
        /// Json serialization
        /// </summary>
        public bool SaveDataToJson<T>(T @object, string address)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(address);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(address))
                {
                    string newFileName = "BankData_" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm") + ".json";
                    string newFilePath = Path.Combine(Path.GetDirectoryName(address), newFileName);
                }

                using (StreamWriter writer = File.CreateText(address))
                {
                    string output = JsonConvert.SerializeObject(@object);
                    writer.Write(output);
                    return true;
                }
            }
            catch
            {
                throw new Exception("Repository exeption");
            }
        }

        /// <summary>
        /// Json deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadAllDataFromJson<T>(string address)
        {
            try
            {

                if (!File.Exists(address))
                {
                    throw new Exception("The file doesn't exist, create a new one, or specify the path to another one");
                }

                using (StreamReader reader = File.OpenText(address))
                {
                    var fileText = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(fileText);
                }
            }
            catch
            {
                throw new Exception("Repository exception");
            }
        }

    }
}