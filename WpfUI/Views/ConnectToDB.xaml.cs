using Bank.Buisness;
using System.Windows;

namespace Bank.Views
{
    /// <summary>
    /// Interaction logic for ConnectToDB.xaml
    /// </summary>
    public partial class ConnectToDB : Window
    {
        private readonly Service _service;

        public ConnectToDB(Service service)
        {
            InitializeComponent();
            _service = service;
        }

        /// <summary>
        /// Click for create/change connection parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
            _service.UpdateConnection($@"{SourceDB.Text}", $@"{NameDB.Text}");

            Close();
        }

    }
}
