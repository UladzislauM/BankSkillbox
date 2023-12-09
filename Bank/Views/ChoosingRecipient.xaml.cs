using Bank.Buisness;
using MarshalsExceptions;
using System;
using System.Linq;
using System.Windows;

namespace Bank.Views
{
    /// <summary>
    /// Interaction logic for ChoosingRecipient.xaml
    /// </summary>
    public partial class ChoosingRecipient : Window
    {
        private readonly Service _service;

        public ChoosingRecipient(Service service)
        {
            InitializeComponent();

            _service = service;
            CreateAnAnonymousClass();
        }

        private void CreateAnAnonymousClass()
        {
            Clients_Box.ItemsSource = _service.Scores.Select(score => new ViewObject
            {
                Id = score.Id,
                FullNameAndTypeScore = $"{score.Client.FirstName} {score.Client.LastName} {score.ScoreType} {score.Balance}"

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
                            MessageBox.Show("Something went wrong");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Field sum is empty.");
                    }
                }
                else
                {
                    MessageBox.Show("Choose recipient.");
                }
            }
            catch (TransactionException)
            {
                MessageBox.Show("Unable to send money");
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
