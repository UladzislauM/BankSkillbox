using Bank.Buisness;
using MarshalsExceptions;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// View for client selection AddClient.xaml
    /// </summary>
    public partial class AddClientView : Window
    {
        private readonly Service _service;
        private readonly ILogger<AddClientView> _logger;

        private Client.Statuses _clientStatus;

        public AddClientView(Service service, ILogger<AddClientView> logger)
        {
            InitializeComponent();

            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Button for adding a client
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _service.CreateNewClient(_clientStatus, ClientFirstName.Text, ClientLastName.Text);
            }
            catch (ClientException ex)
            {
                _logger.LogError("Something went wrong: " + ex.Message);
                MessageBox.Show("Something went wrong: " + ex.Message);
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
