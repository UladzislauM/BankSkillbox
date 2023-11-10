using Microsoft.Extensions.Logging;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Buisness
{

    /// <summary>
    /// Business logic for UladzislauM banking application.
    /// </summary>
    public class Service : MyNotification
    {
        private readonly ILogger<Service> _logger;
        private readonly Repository _repository;

        public event Action<object> SavedJsonObject;
        public event Action<List<Score>> ImportantScores;

        /// <summary>
        /// Table client's focus
        /// </summary>
        public long ClientId;//TODO sometimes create new client with 0 id

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

        public Service(ILogger<Service> logger, Repository repository)
        {
            _logger = logger;
            _repository = repository;

            ClientId = 0;
            ScoreId = 0;

            Scores = new ObservableCollection<Score>();//TODO add read from file or create new
            Clients = new ObservableCollection<Client>();//TODO add read from file or create new

            JsonAddress = "";
        }

        /// <summary>
        /// Create and add to view collection score
        /// </summary>
        /// <param name="scoreType"></param>
        /// <param name="isCapitalization"></param>
        /// <param name="period"></param>
        /// <param name="sum"></param>
        public void CreateNewScore(Score.ScoreTypes scoreType, bool isCapitalization, int period, Decimal sum)
        {
            Score score = new Score(scoreType);

            score.Id = ScoreId;
            ScoreId++;
            DateTime dateTime = DateTime.Now;
            score.DateScore = dateTime;//TODO change format
            score.Deadline = score.DateScore.AddMonths(period);
            score.Balance = sum;
            score.IsСapitalization = isCapitalization;
            score.Client = Clients[(int)ClientId];
            score.IsActive = true;
            if (scoreType == Score.ScoreTypes.Deposit)
            {
                score.IsMoney = false;
            }
            else if (scoreType == Score.ScoreTypes.Credit)
            {
                score.IsMoney = true;
            }

            _logger.LogInformation($"A {score.ScoreType} account has been created for the user {score.Client.FirstName} {score.Client.LastName}");

            Scores.Add(score);
        }

        /// <summary>
        /// Create and add to view collection client
        /// </summary>
        /// <param name="status"></param>
        public void CreateNewClient(Client.Statuses status)
        {
            Client client = new Client(status);

            client.Id = ClientId;
            ClientId++;

            _logger.LogInformation($"Client {client.FirstName} {client.LastName} has been created.");

            Clients.Add(client);
        }

        /// <summary>
        /// Save all data
        /// </summary>
        public void SaveJsonWithAllData()
        {
            try
            {
                UnionData data = new UnionData();

                data.Clients = Clients;
                data.Scores = Scores;

                _logger.LogInformation($"General Json saved");

                SaveJsonData<UnionData>(data);
            }
            catch
            {
                _logger.LogError("General Json don't save");
                throw new Exception("General Json don't save");
            }
        }

        /// <summary>
        /// Save object to filesystem
        /// </summary>
        /// <param name="objectForSaveJson"></param>
        /// <param name="address"></param>
        /// <exception cref="Exception"></exception>
        public bool SaveJsonData<T>(T objectForSaveJson)
        {
            try
            {
                if (JsonAddress == "")
                {
                    string fileName = "BankData_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".json";
                    JsonAddress = $@"{Environment.CurrentDirectory}\\WorkData\\{fileName}";
                }

                bool stateOfSave = _repository.SaveData<T>(objectForSaveJson, JsonAddress);

                if (stateOfSave)
                {
                    SavedJsonObject.Invoke(objectForSaveJson);
                }

                _logger.LogInformation($"Json data saved.");

                return stateOfSave;
            }
            catch
            {
                _logger.LogError($"Json data saved.");
                throw new Exception("Save json data failed");
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
                UnionData unionData = _repository.LoadDataAll<UnionData>(JsonAddress);

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

                _logger.LogInformation($"Json opened.");
            }
            catch
            {
                _logger.LogError($"Open Json failed.");
                throw new Exception("Open Json failed");
            }
        }

        /// <summary>
        /// Check deadline all scores
        /// </summary>
        public void CheckDeadline()
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
                        _logger.LogInformation($"Deposit with id = {currentScore.Id} ended.");
                        currentScore.IsMoney = true;
                    }
                    else if (currentScore.ScoreType == Score.ScoreTypes.Credit)
                    {
                        _logger.LogInformation($"Credit with id = {currentScore.Id} ended.");
                        currentScore.IsMoney = false;
                    }

                    identifiedScores.Add(currentScore);
                }
                //Checking deposit's capitalisation
                else if (currentScore.ScoreType == Score.ScoreTypes.Deposit &&
                    currentScore.IsСapitalization == true &&
                    currentScore.DateLastDividends.AddMonths(1) <= checkingData)
                {
                    currentScore.DateLastDividends = checkingData;
                    currentScore.Balance += Decimal.Multiply(currentScore.Balance, currentScore.Persents);

                    int recipientCollectionIndex = Scores.IndexOf(Scores.First(item => item.Id == currentScore.Id));
                    Scores[recipientCollectionIndex] = currentScore;

                    _logger.LogInformation($"Deposit with id = {currentScore.Id} a calculated capitalization.");
                }
            }

            if (identifiedScores.Count > 0)
            {
                ImportantScores.Invoke(identifiedScores);
            }
        }

        public bool SendMoney(long scoreRecipientId, Decimal sum) //The transaction is out of scope
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

                    _logger.LogInformation($"Money transferred from account id = {senderScore.Id} to account id = {recipientScore.Id}.");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                _logger.LogError("Send money failed.");
                throw new Exception("Send money failed.");
            }

        }

    }

    public struct UnionData
    {
        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Score> Scores { get; set; }
    }
}
