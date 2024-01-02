using Bank.Buisness;
using MarshalsExceptions;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace Bank.Views
{
    /// <summary>
    /// Interaction logic for ChoosingRecipient.xaml
    /// </summary>
    public partial class ChoosingRecipient : Window
    {
        private readonly Service _service;
        private readonly ILogger<ChoosingRecipient> _logger;

        public ChoosingRecipient(Service service, ILogger<ChoosingRecipient> logger)
        {
            InitializeComponent();

            _service = service;
            _logger = logger;

            CreateAnAnonymousClass();
        }

        private void CreateAnAnonymousClass()
        {
            Clients_Box.ItemsSource = _service.Scores.Select(score => new ViewObject
            {
                Id = score.Id,
                FullNameAndTypeScore = $"{_service.Clients[(int)score.ClientId - 1].FirstName}" +
                $" {_service.Clients[(int)score.ClientId - 1].LastName} " +
                $"{score.ScoreType} " +
                $"{score.Balance}"

            }).Where(parameter => parameter.Id != _service.ScoreId);
            Clients_Box.DisplayMemberPath = "FullNameAndTypeScore";
        }

        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Clients_Box.SelectedItem != null)
                {
                    var recipientScore = (ViewObject)Clients_Box.SelectedItem;

                    Decimal sum = new Decimal();

                    if (Sum != null
                        && Decimal.TryParse($"{Sum.Text}", out sum))
                    {
                        bool resualt = _service.SendMoney(recipientScore.Id, sum);

                        if (!resualt)
                        {
                            _logger.LogError("Something went wrong from send money");
                            MessageBox.Show("Something went wrong from send money");
                        }
                    }
                    else
                    {
                        _logger.LogError("Field sum is empty.");
                        MessageBox.Show("Field sum is empty.");
                    }
                }
                else
                {
                    _logger.LogError("Choose recipient.");
                    MessageBox.Show("Choose recipient.");
                }
            }
            catch (TransactionException ex)
            {
                _logger.LogError(ex.Message + " Unable to send money");
                MessageBox.Show(ex.Message + " Unable to send money");
            }

            Close();
        }

    }

    public struct ViewObject
    {
        public long Id { get; set; }
        public string FullNameAndTypeScore { get; set; }
    }
}
