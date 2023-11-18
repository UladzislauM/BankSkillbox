using Bank.Buisness;
using MarshalsExceptions;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// View for client selection AddClient.xaml
    /// </summary>
    public partial class AddClientView : Window
    {
        private readonly Service _service;

        private Client.Statuses _clientStatus;

        public AddClientView(Service service)
        {
            InitializeComponent();

            _service = service;
        }

        /// <summary>
        /// Button for adding a client
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _service.CreateNewClient(_clientStatus);
            }
            catch (ClientException)
            {
                MessageBox.Show("Something went wrong...");
            }

            Close();
        }

        private void ComboBoxItem_Selected_normPeople(object sender, RoutedEventArgs e)
        {
            _clientStatus = Client.Statuses.General;
        }

        private void ComboBoxItem_Selected_VIPPeople(object sender, RoutedEventArgs e)
        {
            _clientStatus = Client.Statuses.VIP;
        }

        private void ComboBoxItem_Selected_corpCkients(object sender, RoutedEventArgs e)
        {
            _clientStatus = Client.Statuses.Corporative;
        }

    }
}
