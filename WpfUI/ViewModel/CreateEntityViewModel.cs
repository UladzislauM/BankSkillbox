using Bank;
using Bank.Buisness;

namespace WpfUI.ViewModel
{
    abstract class CreateEntityViewModel<T> : Notification
    {
        public List<T> Parameteres { get; set; }
        private T _parameter;
        private bool _isActive;
        public DefaultCommand CreateNewEntityCommand { get; }
        public DefaultCommand CancelCommand { get; }

        protected Service _service;

        public CreateEntityViewModel()
        {
            CreateNewEntityCommand = new DefaultCommand(ExecuteCreateNewEntity);
            CancelCommand = new DefaultCommand(ExecuteCancel);
        }

        public T Parameter
        {
            get => _parameter;
            set
            {
                _parameter = value;
                OnPropertyChanged(nameof(Parameter));
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

        protected abstract void ExecuteCreateNewEntity(object? parameter);

        private void ExecuteCancel(object? parameter)
        {
            IsActive = false;
        }

    }
}
