using Bank;
using Bank.Buisness;

namespace WpfUI.ViewModel.Notifications
{
    abstract class DialogBlockViewModel : Notification
    {
        private bool _isActive;
        public DefaultCommand CancelCommand { get; }
        protected Service _service;

        public DialogBlockViewModel(Service service)
        {
            CancelCommand = new DefaultCommand(ExecuteCancel);
            _service = service;
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

        private void ExecuteCancel(object? parameter)
        {
            IsActive = false;
        }

    }
}
