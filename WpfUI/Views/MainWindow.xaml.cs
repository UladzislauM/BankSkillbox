using Bank.Buisness;
using MarshalsExceptions;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace Bank
{
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> _logger;

        public MainWindow()
        {
            InitializeComponent();

            ILogger<Service> servLogger = new LoggerFactory().CreateLogger<Service>();
            _logger = new LoggerFactory().CreateLogger<MainWindow>();
        }

        /// <summary>
        /// Event processing for show important collections
        /// </summary>
        /// <param name="scores"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void _service_ImportantScores(List<Score> scores)
        {
            foreach (Score score in scores)
            {
                MessageBox.Show("Pleace, pay attention for score with id = " + score.Id);
            }
        }

        /// <summary>
        /// Check deadline all scores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_TestDep(object sender, RoutedEventArgs e)
        {
            try
            {
                //_service.CheckDeadline();
            }
            catch (ScoreException ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
            }
        }

        /// <summary>
        /// Action for the "Send money from:" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_SendMoney(object sender, RoutedEventArgs e)
        {
            //ChoosingRecipient choosingRecipient = new ChoosingRecipient(_service, _logger);

            //choosingRecipient.Show();

            this.Send_Money_Button.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Close main window and app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) => Close();

    }
}
