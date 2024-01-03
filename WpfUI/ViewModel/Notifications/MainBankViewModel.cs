using Bank.Buisness;
using MarshalsExceptions;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace Bank
{
    internal class MainBankViewModel : Notification
    {
        private const string DefaultExtJson = ".json";
        private const string FilterJson = "JSON Files|*.json|All Files|*.*";
        private const string SaveTitleJson = "Сохранить файл";
        private const string OpenTitleJson = "Открыть файл";

        private readonly ILogger<MainBankViewModel> _logger;
        private ErrorBankViewModel _errorBankViewModel;
        private readonly Service _service;
        private readonly IDialogService _dialogService;

        public DefaultCommand ExecuteLoadAllDataFromDb { get; }
        public DefaultCommand ExecuteSaveAllDataToDb { get; }
        public DefaultCommand ExecuteClearAllData { get; }
        public DefaultCommand ExecuteSaveJson { get; }
        public DefaultCommand ExecuteOpenJson { get; }
        public DefaultCommand ViewAllClientsCommand { get; }
        public DefaultCommand ViewAllScoresCommand { get; }
        public DefaultCommand ViewGeneralClientsCommand { get; }
        public DefaultCommand ViewVIPClientsCommand { get; }
        public DefaultCommand ViewCorpClientsCommand { get; }
        public DefaultCommand ViewGeneralScoresCommand { get; }
        public DefaultCommand ViewVIPScoresCommand { get; }
        public DefaultCommand ViewCorpScoresCommand { get; }
        public DefaultCommand ViewGeneralDepositsCommand { get; }
        public DefaultCommand ViewVIPDepositsCommand { get; }
        public DefaultCommand ViewCorpDepositsCommand { get; }
        public DefaultCommand ViewGeneralCreditsCommand { get; }
        public DefaultCommand ViewVIPCreditsCommand { get; }
        public DefaultCommand ViewCorpCreditsCommand { get; }

        private ObservableCollection<Client> _clients;
        private ObservableCollection<Score> _scores;
        private string _errorMessage;
        private string _selectedFilePath;

        public MainBankViewModel(ErrorBankViewModel errorBankViewModel,
            Service service,
            ILogger<MainBankViewModel> logger,
            IDialogService dialogService)
        {
            ExecuteLoadAllDataFromDb = new DefaultCommand(ExecuteLoadAllDataFromDbCommand, CanExecute);
            ExecuteSaveAllDataToDb = new DefaultCommand(ExecuteSaveAllDataToDbCommand, CanExecute);
            ExecuteClearAllData = new DefaultCommand(ExecuteClearAllDataCommand, CanExecute);
            ExecuteSaveJson = new DefaultCommand(ExecuteSaveJsonCommand, CanExecute);
            ExecuteOpenJson = new DefaultCommand(ExecuteOpenJsonCommand, CanExecute);
            ViewAllClientsCommand = new DefaultCommand(ExecuteViewAllClientsCommand);
            ViewAllScoresCommand = new DefaultCommand(ExecuteViewAllScoresCommand);
            ViewGeneralClientsCommand = new DefaultCommand(ExecuteViewGeneralClientsCommand);
            ViewVIPClientsCommand = new DefaultCommand(ExecuteViewVIPClientsCommand);
            ViewCorpClientsCommand = new DefaultCommand(ExecuteViewCorpClientsCommand);
            ViewGeneralScoresCommand = new DefaultCommand(ExecuteViewGeneralScoresCommand);
            ViewVIPScoresCommand = new DefaultCommand(ExecuteViewVIPScoresCommand);
            ViewCorpScoresCommand = new DefaultCommand(ExecuteViewCorpScoresCommand);
            ViewGeneralDepositsCommand = new DefaultCommand(ExecuteViewGeneralDepositsCommand);
            ViewVIPDepositsCommand = new DefaultCommand(ExecuteViewVIPDepositsCommand);
            ViewCorpDepositsCommand = new DefaultCommand(ExecuteViewCorpDepositsCommand);
            ViewGeneralCreditsCommand = new DefaultCommand(ExecuteViewGeneralCreditsCommand);
            ViewVIPCreditsCommand = new DefaultCommand(ExecuteViewVIPCreditsCommand);
            ViewCorpCreditsCommand = new DefaultCommand(ExecuteViewCorpCreditsCommand);

            _errorBankViewModel = errorBankViewModel;
            _service = service;
            _logger = logger;
            _dialogService = dialogService;
        }

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        public ObservableCollection<Score> Scores
        {
            get => _scores;
            set
            {
                _scores = value;
                OnPropertyChanged(nameof(Scores));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ErrorBankViewModel ErrorBankViewModel
        {
            get => _errorBankViewModel;
            set
            {
                _errorBankViewModel = value;
                OnPropertyChanged(nameof(ErrorBankViewModel));
            }
        }

        public string SelectedFilePath
        {
            get => _selectedFilePath;
            set
            {
                if (_selectedFilePath != value)
                {
                    _selectedFilePath = value;
                    OnPropertyChanged(nameof(SelectedFilePath));
                }
            }
        }

        /// <summary>
        /// Executes the command to load all data from the database, including clients and scores.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        public void ExecuteLoadAllDataFromDbCommand(object? parameter)
        {
            try
            {
                Clients = new ObservableCollection<Client>(_service.LoadAllClientsFromDB());
                Scores = new ObservableCollection<Score>(_service.LoadAllScoresFromDB());

                _service.Scores = _service.CheckDeadline(_service.Scores); //TODO view deadLine not all data change

                PrepareInfoMessage("All data loaded");
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to save all data to the database.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void ExecuteSaveAllDataToDbCommand(object? parameter)
        {
            try
            {
                _service.SaveAllDataToDB();

                PrepareInfoMessage("All data saved");
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to clear all data, including clients and scores.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void ExecuteClearAllDataCommand(object? parameter)
        {
            try
            {
                Clients.Clear();
                Scores.Clear();

                _service.Clients.Clear();
                _service.Scores.Clear();

                _service.ClientId = 1;
                _service.ScoreId = 1;

                PrepareInfoMessage("All data cleared");
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to save data to a JSON file.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void ExecuteSaveJsonCommand(object? parameter)
        {
            try
            {
                _dialogService.SaveFileDialog(DefaultExtJson, FilterJson, SaveTitleJson);

                UnionData unionData = new UnionData { Clients = Clients, Scores = Scores };

                _service.SaveJsonWithAllData(unionData, _dialogService.FilePath);

                PrepareInfoMessage("Json saved");
            }
            catch (JsonException ex)
            {
                PrepareExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to open data from a JSON file.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void ExecuteOpenJsonCommand(object? parameter)
        {
            try
            {
                _dialogService.OpenFileDialog(DefaultExtJson, FilterJson, OpenTitleJson);

                UnionData unionData = _service.OpenJsonData(_dialogService.FilePath);

                if (unionData.Clients != null)
                {
                    Clients = new ObservableCollection<Client>(unionData.Clients);
                }
                if (unionData.Scores != null)
                {
                    Scores = new ObservableCollection<Score>(unionData.Scores);
                }

                _service.Scores = _service.CheckDeadline(_service.Scores); //TODO view deadLine not all data change

                PrepareInfoMessage("Json opened");
            }
            catch (JsonException ex)
            {
                PrepareExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view all clients.
        /// </summary>
        private void ExecuteViewAllClientsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView();
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view all scores.
        /// </summary>
        private void ExecuteViewAllScoresCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(null, false, null, true);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view general clients.
        /// </summary>
        private void ExecuteViewGeneralClientsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.General);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view VIP clients.
        /// </summary>
        private void ExecuteViewVIPClientsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.VIP);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view corporate clients.
        /// </summary>
        private void ExecuteViewCorpClientsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.Corporative);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view general scores.
        /// </summary>
        private void ExecuteViewGeneralScoresCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.General, true);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view VIP scores.
        /// </summary>
        private void ExecuteViewVIPScoresCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.VIP, true);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view corporate scores.
        /// </summary>
        private void ExecuteViewCorpScoresCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.Corporative, true);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view general deposits.
        /// </summary>
        private void ExecuteViewGeneralDepositsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.General, true, Score.ScoreTypes.Deposit);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view VIP deposits.
        /// </summary>
        private void ExecuteViewVIPDepositsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.VIP, true, Score.ScoreTypes.Deposit);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view corporate deposits.
        /// </summary>
        private void ExecuteViewCorpDepositsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.Corporative, true, Score.ScoreTypes.Deposit);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view general credits.
        /// </summary>
        private void ExecuteViewGeneralCreditsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.General, true, Score.ScoreTypes.Credit);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view VIP credits.
        /// </summary>
        private void ExecuteViewVIPCreditsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.VIP, true, Score.ScoreTypes.Credit);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Executes the command to view corporate credits.
        /// </summary>
        private void ExecuteViewCorpCreditsCommand(object? parameter)
        {
            try
            {
                WritePartCollectionToView(Client.Statuses.Corporative, true, Score.ScoreTypes.Credit);
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        private void PrepareInfoMessage(string message)
        {
            PrepareMessageToShow($"{message}");
        }

        private void PrepareExceptionMessage(Exception ex)
        {
            PrepareMessageToShow($"Something went wrong: {ex.Message}");
        }

        private void PrepareMessageToShow(string message)
        {
            ErrorBankViewModel.ErrorMessage = $"{message}";
        }

        private void WritePartCollectionToView(Client.Statuses? status = null, bool isScore = false, Score.ScoreTypes? typeScore = null, bool isAllScores = false)
        {
            try
            {
                Scores.Clear();
                Clients.Clear();

                if (status == null)
                {
                    Clients = new ObservableCollection<Client>(_service.Clients);
                }
                else
                {
                    List<Client> generalClients = _service.Clients.Where(parameter => parameter.Status == status).ToList();
                    Clients = new ObservableCollection<Client>(generalClients);
                }

                if (isScore)
                {
                    if (typeScore.HasValue)
                    {
                        List<Score> scoresGeneralPeoples = _service.Scores.Where(parameter
                            => _service.Clients[(int)parameter.ClientId - 1].Status == status
                            && parameter.ScoreType == typeScore).ToList();
                        Scores = new ObservableCollection<Score>(scoresGeneralPeoples);
                    }
                    else
                    {
                        List<Score> scoresGeneralPeoples = _service.Scores.Where(parameter
                            => _service.Clients[(int)parameter.ClientId - 1].Status == status).ToList();
                        Scores = new ObservableCollection<Score>(scoresGeneralPeoples);
                    }
                }
                if (isAllScores)
                {
                    Scores = new ObservableCollection<Score>(_service.Scores);
                }
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

        private void WriteEntityToView<T>(long id)
        {
            try
            {
                if (typeof(T) == typeof(Client))
                {
                    List<Score> scores = _service.Scores.Where(parameter => parameter.ClientId == id).ToList();
                    Scores = new ObservableCollection<Score>(scores);
                }
                else if (typeof(T) == typeof(Score))
                {
                    List<Score> scores = _service.Scores.Where(parameter => parameter.Id == id).ToList();
                    List<Client> clients = _service.Clients.Where(paremetr => paremetr.Id == scores[0].ClientId).ToList();
                    Clients = new ObservableCollection<Client>(clients);
                }
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

    }
}
