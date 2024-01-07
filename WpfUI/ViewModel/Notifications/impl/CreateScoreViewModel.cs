using Bank;
using Bank.Buisness;
using static Bank.Score;

namespace WpfUI.ViewModel.Notifications
{
    class CreateScoreViewModel : CreateEntityViewModel<ScoreTypes>
    {
        private bool _isCapitalization;
        private int _period;
        private Decimal _sum;
        private List<Client> _clients;
        private Client _selectedClient;

        public CreateScoreViewModel(Service service)
        {
            Parameteres = Enum.GetValues(typeof(ScoreTypes)).Cast<ScoreTypes>().ToList();
            _service = service;
        }

        public bool IsCapitalization
        {
            get => _isCapitalization;
            set
            {
                _isCapitalization = value;
                OnPropertyChanged(nameof(IsCapitalization));
            }
        }

        public int Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged(nameof(Period));
            }
        }

        public Decimal Sum
        {
            get => _sum;
            set
            {
                _sum = value;
                OnPropertyChanged(nameof(Sum));
            }
        }

        public List<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }

        protected override void ExecuteCreateNewEntity(object? parameter)
        {
            _service.CreateNewScore(SelectedClient, Parameter, IsCapitalization, Period, Sum);
            IsActive = false;
        }
    }
}
