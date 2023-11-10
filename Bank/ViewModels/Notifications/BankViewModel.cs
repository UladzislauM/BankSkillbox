using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace Bank.ViewModels.Notifications
{
    internal class BankViewModel : MyNotification
    {
        private readonly ILogger<BankViewModel> _logger;

        private ObservableCollection<Score> _scores;
        private ObservableCollection<Client> _peoples;


        public BankViewModel(ILogger<BankViewModel> logger)
        {
            _logger = logger;
            _scores = new ObservableCollection<Score>();
            _peoples = new ObservableCollection<Client>();
        }

        public ObservableCollection<Client> Peoples
        {
            get => _peoples;
            set
            {
                _peoples = value;
                OnPropertyChanged(nameof(Peoples));
            }
        }



    }
}
