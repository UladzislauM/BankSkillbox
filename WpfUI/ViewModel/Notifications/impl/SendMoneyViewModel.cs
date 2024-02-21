using Bank;
using Bank.Buisness;
using WpfUI.Models;

namespace WpfUI.ViewModel.Notifications.impl
{
    class SendMoneyViewModel : DialogBlockViewModel
    {
        private List<SendMoneyModel> _sendMoneyModels;
        private SendMoneyModel _fromScore;
        private SendMoneyModel _toScore;
        private Decimal _sum;
        public DefaultCommand ExecuteSendMoney { get; }

        public SendMoneyViewModel(Service service) : base(service)
        {
            ExecuteSendMoney = new DefaultCommand(ExecuteSendMoneyCommand);
        }

        public SendMoneyModel FromScore
        {
            get => _fromScore;
            set
            {
                _fromScore = value;
                OnPropertyChanged(nameof(FromScore));
            }
        }

        public SendMoneyModel ToScore
        {
            get => _toScore;
            set
            {
                _toScore = value;
                OnPropertyChanged(nameof(ToScore));
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

        public List<SendMoneyModel> SendMoneyModels
        {
            get => _sendMoneyModels;
            set
            {
                _sendMoneyModels = value;
                OnPropertyChanged(nameof(SendMoneyModels));
            }
        }

        private void ExecuteSendMoneyCommand(object? obj)
        {
            _service.SendMoney(FromScore.Score.Id, ToScore.Score.Id, Sum);
            IsActive = false;
        }

    }
}
