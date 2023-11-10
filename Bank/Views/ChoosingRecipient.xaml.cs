using Bank.Buisness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            catch
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
