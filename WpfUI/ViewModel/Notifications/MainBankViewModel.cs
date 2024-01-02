using Bank.Buisness;
using MarshalsExceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
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

        public DefaultCommand ExecuteLoadAllDataFromDb { get; private set; }
        public DefaultCommand ExecuteSaveAllDataToDb { get; set; }
        public DefaultCommand ExecuteClearAllData { get; set; }
        public DefaultCommand ExecuteSaveJson { get; }
        public DefaultCommand ExecuteOpenJson { get; }

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

        public void ExecuteLoadAllDataFromDbCommand(object? parameter)
        {
            try
            {
                Clients = _service.LoadAllClientsFromDB();
                Scores = _service.LoadAllScoresFromDB();

                Scores = _service.CheckDeadline(Scores);

                PrepareInfoMessage("All data loaded");
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

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

        private void ExecuteClearAllDataCommand(object? parameter)
        {
            try
            {
                Clients.Clear();
                Scores.Clear();

                _service.ClientId = 1;
                _service.ScoreId = 1;

                PrepareInfoMessage("All data cleared");
            }
            catch (Exception ex)
            {
                PrepareExceptionMessage(ex);
            }
        }

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

        private void ExecuteOpenJsonCommand(object? parameter)
        {
            try
            {
                _dialogService.OpenFileDialog(DefaultExtJson, FilterJson, OpenTitleJson);

                UnionData unionData = _service.OpenJsonData(_dialogService.FilePath);

                if (unionData.Clients != null)
                {
                    Clients = unionData.Clients;
                    _service.ClientId = Clients[Clients.Count - 1].Id + 1;
                }
                if (unionData.Scores != null)
                {
                    Scores = unionData.Scores;
                    _service.ScoreId = Scores[Scores.Count - 1].Id + 1;
                }

                Scores = _service.CheckDeadline(Scores);

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

    }
}
