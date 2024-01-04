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
        /// Action for the "Add score" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_AddScore(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Создать депозит? При нажатии кнопки нет создастся кредит.",
                "Создание счета",
                MessageBoxButton.YesNoCancel);

            //CreateScore createScore = new CreateScore(_service, _logger);

            //switch (result)
            //{
            //    case MessageBoxResult.Yes:
            //        createScore.capitalizationCheckBox.Visibility = Visibility.Visible;
            //        createScore.ShowDialog();

            //        break;
            //    case MessageBoxResult.No:
            //        createScore.capitalizationCheckBox.Visibility = Visibility.Hidden;
            //        createScore.ShowDialog();

            //        return;
            //}

            //WriteEntityToView<Client>(_service.ClientId);

            MessageBox.Show("Score added");
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
        /// Action for the "All clients" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_AllClients(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView();
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

        private void WritePartCollectionToView(Client.Statuses? status = null, bool isScore = false, Score.ScoreTypes? typeScore = null, bool isAllScores = false)
        {
            //try
            //{
            //    dgScoresList.ItemsSource = null;
            //    dgClientsList.ItemsSource = null;

            //    if (status == null)
            //    {
            //        dgClientsList.ItemsSource = _service.Clients;
            //    }
            //    else
            //    {
            //        List<Client> generalClients = _service.Clients.Where(parameter => parameter.Status == status).ToList();
            //        dgClientsList.ItemsSource = new ObservableCollection<Client>(generalClients);
            //    }

            //    if (isScore)
            //    {
            //        if (typeScore.HasValue)
            //        {
            //            List<Score> scoresGeneralPeoples = _service.Scores.Where(parameter
            //                => _service.Clients[(int)parameter.ClientId - 1].Status == status
            //                && parameter.ScoreType == typeScore).ToList();
            //            dgScoresList.ItemsSource = new ObservableCollection<Score>(scoresGeneralPeoples);
            //        }
            //        else
            //        {
            //            List<Score> scoresGeneralPeoples = _service.Scores.Where(parameter
            //                => _service.Clients[(int)parameter.ClientId - 1].Status == status).ToList();
            //            dgScoresList.ItemsSource = new ObservableCollection<Score>(scoresGeneralPeoples);
            //        }
            //    }
            //    if (isAllScores)
            //    {
            //        ObservableCollection<Score> allScores = _service.Scores;
            //        dgScoresList.ItemsSource = allScores;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Something went wrong: " + ex.Message);
            //}
        }

        private void WriteEntityToView<T>(long id)
        {
            try
            {
                if (typeof(T) == typeof(Client))
                {
                    //List<Score> scores = _service.Scores.Where(parameter => parameter.ClientId == id).ToList();
                    //dgScoresList.ItemsSource = new ObservableCollection<Score>(scores);
                }
                else if (typeof(T) == typeof(Score))
                {
                    //List<Score> scores = _service.Scores.Where(parameter => parameter.Id == id).ToList();
                    //List<Client> clients = _service.Clients.Where(paremetr => paremetr.Id == scores[0].ClientId).ToList();
                    //dgClientsList.ItemsSource = new ObservableCollection<Client>(clients);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
            }
        }

    }
}
