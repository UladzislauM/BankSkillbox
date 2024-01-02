namespace Bank
{
    internal class ErrorBankViewModel : Notification
    {
        private string errorMessage;
        public DefaultCommand ExecuteVisibalBlock { get; set; }

        public ErrorBankViewModel()
        {
            ExecuteVisibalBlock = new DefaultCommand(ExecuteVisibleCommand, CanExecute);
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (value != errorMessage)
                {
                    errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        private void ExecuteVisibleCommand(object? parameter)
        {
            ErrorMessage = string.Empty;
        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

    }
}
