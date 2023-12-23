using MarshalsExceptions;
using NLog;
using System.Collections.ObjectModel;

namespace Bank.Buisness
{

    /// <summary>
    /// Business logic for UladzislauM banking application.
    /// </summary>
    public class Service : MyNotification //TODO How work with Json & DB?
    {
        public string DBName { get; set; }
        public string DBSource { get; set; }
        private readonly NLog.Logger _logger;
        private readonly RepositoryForDB _repositoryForDB;
        private readonly RepositoryForJson _repositoryForJson;

        public event Action<object> SavedJsonObject;
        public event Action<List<Score>> ImportantScores;

        /// <summary>
        /// Table client's focus
        /// </summary>
        public long ClientId;

        /// <summary>
        /// Table score's focus
        /// </summary>
        public long ScoreId;

        /// <summary>
        /// Puth to save location
        /// </summary>
        public string JsonAddress;

        public ObservableCollection<Score> Scores;
        public ObservableCollection<Client> Clients;

        public Service()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _repositoryForDB = new RepositoryForDB();
            _repositoryForJson = new RepositoryForJson();

            ClientId = 1;
            ScoreId = 1;

            Scores = new ObservableCollection<Score>();
            Clients = new ObservableCollection<Client>();

            JsonAddress = "";
        }

        /// <summary>
        /// Create and add to view collection score
        /// </summary>
        /// <param name="scoreType"></param>
        /// <param name="isCapitalization"></param>
        /// <param name="period"></param>
        /// <param name="sum"></param>
        /// <exception cref="DBException"></exception>
        /// <exception cref="ScoreException"></exception>
        public void CreateNewScore(Score.ScoreTypes scoreType, bool isCapitalization, int period, Decimal sum)
        {
            try
            {
                Score score = new Score(scoreType);

                int currentScoreId = (int)Scores[Scores.Count - 1].Id;

                score.Id = currentScoreId + 1;
                score.Deadline = score.DateScore.AddMonths(period);
                score.Balance = sum;
                score.IsCapitalization = isCapitalization;
                //score.Client = Clients[(int)ClientId - 1];
                score.ClientId = (int)ClientId;
                score.IsActive = true;

                if (Clients[(int)ClientId - 1].Scores == null)
                {
                    Clients[(int)ClientId - 1].Scores = new List<Score>() { score };
                }
                else
                {
                    Clients[(int)ClientId - 1].Scores.Add(score);
                }

                if (scoreType == Score.ScoreTypes.Deposit)
                {
                    score.IsMoney = false;
                }
                else if (scoreType == Score.ScoreTypes.Credit)
                {
                    score.IsMoney = true;
                }

                Scores.Add(score);

                _logger.Info($"A {score.ScoreType} account has been created for the user with id = {score.ClientId}");
            }
            catch (DBException)
            {
                _logger.Error("Something went wrong with DB");
                throw new DBException("Something went wrong with DB");
            }
            catch
            {
                _logger.Error("Something went wrong with the creation of the Score");
                throw new ScoreException("Something went wrong with the creation of the Score");
            }
        }

        /// <summary>
        /// Update score infirmation into DB.
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="DBException"></exception>
        public void UpdateScoreIntoDB(Score score)
        {
            try
            {
                _repositoryForDB.UpdateEntityIntoDB(score, "DBConnectionMS");
                _repositoryForDB.UpdateEntityIntoDB(score, "DBConnectionPSQL");
            }
            catch (DBException)
            {
                _logger.Error("Something went wrong with DB");
                throw new DBException("Something went wrong with DB");
            }
        }

        /// <summary>
        /// Create and add to view collection client
        /// </summary>
        /// <param name="status"></param>
        /// <exception cref="DBException"></exception>
        /// <exception cref="ClientException"></exception>
        public void CreateNewClient(Client.Statuses status, string firstName, string lastName)
        {
            try
            {
                Client client = new Client(status);

                int currentClientId = Clients.Count;

                client.Id = ClientId++;
                client.FirstName = firstName;
                client.LastName = lastName;
                client.History = "";

                Clients.Add(client);

                _logger.Info($"Client {client.FirstName} {client.LastName} has been created.");
            }
            catch (DBException)
            {
                _logger.Error("Something went wrong with DB");
                throw new DBException("Something went wrong with DB");
            }
            catch
            {
                _logger.Error("Something went wrong with the creation of the Client");
                throw new ClientException("Something went wrong with the creation of the Client");
            }
        }

        /// <summary>
        /// Update client infirmation into DB.
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="DBException"></exception>
        public void UpdateClientIntoDB(Client client)
        {
            try
            {
                _repositoryForDB.UpdateEntityIntoDB(client, "DBConnectionMS");
                _repositoryForDB.UpdateEntityIntoDB(client, "DBConnectionPSQL");
            }
            catch (DBException)
            {
                _logger.Error("Something went wrong with DB");
                throw new DBException("Something went wrong with DB");
            }
        }

        /// <summary>
        /// Save all data to the DB
        /// </summary>
        /// <exception cref="DBException"></exception>
        public void SaveAllDataToDB()
        {
            try
            {
                _repositoryForDB.UpdateEntitiesIntoDB(Clients.ToList(), "DBConnectionMS");
                //_repositoryForDB.UpdateEntitiesIntoDB(Clients.ToList(), "DBConnectionPSQL");

                _repositoryForDB.UpdateEntitiesIntoDB(Scores.ToList(), "DBConnectionMS");
                //_repositoryForDB.UpdateEntitiesIntoDB(Scores.ToList(), "DBConnectionPSQL");

                _logger.Info($"General data saved in the DB");
            }
            catch
            {
                _logger.Error("Something went wrong while saveing data to the DB");
                throw new DBException("Something went wrong while saveing data to the DB");
            }
        }

        /// <summary>
        /// Loading all clients from the DB.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public ObservableCollection<Client> LoadAllClientsFromDB()
        {
            try
            {
                ObservableCollection<Client> clients = new ObservableCollection<Client>(_repositoryForDB.FindEntitiesFromDB<Client>("DBConnectionMS"));

                if (clients.Count != 0)
                {
                    Clients = clients;
                    ClientId = clients[clients.Count - 1].Id;
                    ClientId++;
                }

                return clients;
            }
            catch
            {
                throw new DBException("Load Clients exception");
            }
        }

        /// <summary>
        /// Loading all scores from the DB.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DBException"></exception>
        public ObservableCollection<Score> LoadAllScoresFromDB()
        {
            try
            {
                ObservableCollection<Score> scores = new ObservableCollection<Score>(_repositoryForDB.FindEntitiesFromDB<Score>("DBConnectionMS"));

                if (scores.Count != 0)
                {
                    Scores = scores;
                    ScoreId = scores[scores.Count - 1].Id;
                    ScoreId++;
                }

                return scores;
            }
            catch
            {
                throw new DBException("Load Scores exception");
            }
        }

        /// <summary>
        /// Save all data to Json
        /// </summary>
        /// <exception cref="JsonException"></exception>
        public void SaveJsonWithAllData()
        {
            try
            {
                UnionData data = new UnionData();

                data.Clients = Clients;
                data.Scores = Scores;

                SaveJsonData<UnionData>(data);

                _logger.Info($"General Json saved");
            }
            catch
            {
                _logger.Error("General Json don't save");
                throw new JsonException("General Json don't save");
            }
        }

        /// <summary>
        /// Save object to filesystem
        /// </summary>
        /// <param name="objectForSaveJson"></param>
        /// <param name="address"></param>
        /// <exception cref="JsonException"></exception>
        public bool SaveJsonData<T>(T objectForSaveJson)
        {
            try
            {
                if (JsonAddress == "")
                {
                    string fileName = "BankData_" + DateTime.Now.ToString("yyyy-MM-dd_HH.mm") + ".json";
                    JsonAddress = $@"{Environment.CurrentDirectory}\\WorkData\\{fileName}";
                }

                bool stateOfSave = _repositoryForJson.SaveDataToJson<T>(objectForSaveJson, JsonAddress);

                if (stateOfSave)
                {
                    SavedJsonObject.Invoke(objectForSaveJson);
                }

                _logger.Info($"Json data saved.");

                return stateOfSave;
            }
            catch
            {
                _logger.Error($"Save json data failed");
                throw new JsonException("Save json data failed");
            }
        }

        /// <summary>
        /// Opem object from filesystem
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void OpenJsonData()
        {
            try
            {
                UnionData unionData = _repositoryForJson.LoadAllDataFromJson<UnionData>(JsonAddress);

                if (unionData.Clients != null)
                {
                    Clients = unionData.Clients;
                    ClientId = Clients[Clients.Count - 1].Id + 1;
                }
                if (unionData.Scores != null)
                {
                    Scores = unionData.Scores;
                    ScoreId = Scores[Scores.Count - 1].Id + 1;
                }

                _logger.Info($"Json opened.");
            }
            catch
            {
                _logger.Error($"Open Json failed.");
                throw new JsonException("Open Json failed");
            }
        }

        /// <summary>
        /// Check deadline all scores
        /// </summary>
        public void CheckDeadline()
        {
            try
            {
                DateTime currentData = DateTime.Now;
                List<Score> identifiedScores = new List<Score>();

                foreach (Score currentScore in Scores)
                {
                    DateTime checkingData = currentScore.Deadline;

                    if (currentData >= checkingData)
                    {
                        currentScore.IsActive = false;

                        if (currentScore.ScoreType == Score.ScoreTypes.Deposit)
                        {
                            _logger.Info($"Deposit with id = {currentScore.Id} ended.");
                            currentScore.IsMoney = true;
                        }
                        else if (currentScore.ScoreType == Score.ScoreTypes.Credit)
                        {
                            _logger.Info($"Credit with id = {currentScore.Id} ended.");
                            currentScore.IsMoney = false;
                        }

                        identifiedScores.Add(currentScore);
                    }
                    //Checking deposit's capitalisation
                    else if (currentScore.ScoreType == Score.ScoreTypes.Deposit &&
                        currentScore.IsCapitalization == true &&
                        currentScore.DateLastDividends.AddMonths(1) <= checkingData)
                    {
                        currentScore.DateLastDividends = checkingData;
                        currentScore.Balance += Decimal.Multiply(currentScore.Balance, currentScore.Percent);

                        int recipientCollectionIndex = Scores.IndexOf(Scores.First(item => item.Id == currentScore.Id));
                        Scores[recipientCollectionIndex] = currentScore;

                        _logger.Info($"Deposit with id = {currentScore.Id} a calculated capitalization.");
                    }
                }

                if (identifiedScores.Count > 0)
                {
                    ImportantScores.Invoke(identifiedScores);
                }
            }
            catch
            {
                _logger.Error("Score verification failed");
                throw new ScoreException("Score verification failed");
            }
        }

        /// <summary>
        /// Method for transfer money among scores
        /// </summary>
        /// <param name="scoreRecipientId"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        /// <exception cref="TransactionException"></exception>
        public bool SendMoney(long scoreRecipientId, Decimal sum)
        {
            try
            {
                Score senderScore = Scores.Where(paremeter => paremeter.Id == ScoreId).First();

                if (senderScore.Balance >= sum)
                {
                    Score recipientScore = Scores.Where(paremetr => paremetr.Id == scoreRecipientId).First();

                    senderScore.Balance -= sum;

                    int senderCollectionIndex = Scores.IndexOf(Scores.First(item => item.Id == ScoreId));
                    Scores[senderCollectionIndex] = senderScore;

                    recipientScore.Balance += sum;

                    int recipientCollectionIndex = Scores.IndexOf(Scores.First(item => item.Id == scoreRecipientId));
                    Scores[recipientCollectionIndex] = recipientScore;

                    _logger.Info($"Money transferred from account id = {senderScore.Id} to account id = {recipientScore.Id}.");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                _logger.Error("Send money failed.");
                throw new TransactionException("Send money failed.");
            }
        }

    }

    public struct UnionData
    {
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Score> Scores { get; set; }
    }
}
