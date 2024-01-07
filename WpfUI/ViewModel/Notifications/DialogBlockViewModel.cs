using Bank;

namespace WpfUI.ViewModel.Notifications
{
    abstract class DialogBlockViewModel : Notification
    {
        private bool _isActive;
        public DefaultCommand CancelCommand { get; }

        public DialogBlockViewModel()
        {
            CancelCommand = new DefaultCommand(ExecuteCancel);
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
