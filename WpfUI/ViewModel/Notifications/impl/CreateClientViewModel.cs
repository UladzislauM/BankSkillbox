using Bank.Buisness;
using static Bank.Client;

namespace WpfUI.ViewModel.Notifications
{
    internal class CreateClientViewModel : CreateEntityViewModel<Statuses>
    {
        private string _firstName;
        private string _lastName;

        public CreateClientViewModel(Service service)
        {
            Parameteres = Enum.GetValues(typeof(Statuses)).Cast<Statuses>().ToList();
            _service = service;
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

        protected override void ExecuteCreateNewEntity(object? parameter)
        {
            _service.CreateNewClient(Parameter, FirstName, LastName);
            IsActive = false;
        }

    }
}
