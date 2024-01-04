using Bank;
using Bank.Buisness;
using static Bank.Client;

namespace WpfUI.ViewModel.Notifications
{
    internal class AddClientViewModel : Notification
    {
        public List<Client.Statuses> Statuses { get; }
        private Client.Statuses _status;
        private string _firstName;
        private string _lastName;
        private bool _isActive;
        public DefaultCommand CreateNewClientCommand { get; }
        public DefaultCommand CancelCommand { get; }

        private Service _service;

        public AddClientViewModel(Service service)
        {
            CreateNewClientCommand = new DefaultCommand(ExecuteCreateNewClient);
            CancelCommand = new DefaultCommand(ExecuteCancel);
            _service = service;
            Statuses = Enum.GetValues(typeof(Statuses)).Cast<Statuses>().ToList();
        }

        public Client.Statuses Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        private void ExecuteCreateNewClient(object? parameter)
        {
            _service.CreateNewClient(Status, FirstName, LastName);
            IsActive = false;
        }

        private void ExecuteCancel(object? parameter)
        {
            IsActive = false;
        }

    }
}
