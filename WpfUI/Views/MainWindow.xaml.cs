using Bank.Buisness;
using Bank.Views;
using MarshalsExceptions;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bank
{
    public partial class MainWindow : Window
    {
        private readonly Service _service;

        public MainWindow()
        {
            InitializeComponent();
            _service = new Service();

            _service.SavedJsonObject += _service_SavedJsonObject;
            _service.ImportantScores += _service_ImportantScores;
        }

        /// <summary>
        /// Action when selecting a line with General clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_GeneralClients(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.General);
        }

        /// <summary>
        /// Action when selecting a line with VIP clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_VIPClients(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.VIP);
        }

        /// <summary>
        /// Action when selecting a line with corparative clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_CorpClients(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.Corporative);
        }

        /// <summary>
        /// Action when clicking the button with general client accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_Scores(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.General, true);

        }

        /// <summary>
        /// Action when clicking the button with corporate VIP accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_VIPScores(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.VIP, true);
        }

        /// <summary>
        /// Action when clicking the button with corporate client accounts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_CorpScores(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.Corporative, true);
        }

        /// <summary>
        /// Action when clicking the button with general client deposits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_Deposits(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.General, true, Score.ScoreTypes.Deposit);
        }

        /// <summary>
        /// Action when clicking the button with VIP client deposits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_VIPDeposits(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.VIP, true, Score.ScoreTypes.Deposit);
        }

        /// <summary>
        /// Action when clicking the button with corporate client deposits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_CorpDeposits(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.Corporative, true, Score.ScoreTypes.Deposit);
        }

        /// <summary>
        /// Action when clicking the button with general client credits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_Credits(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.General, true, Score.ScoreTypes.Credit);
        }

        /// <summary>
        /// Action when clicking the button with VIP client credits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_VIPCredits(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.VIP, true, Score.ScoreTypes.Credit);
        }

        /// <summary>
        /// Action when clicking the button with corporative client credits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Selected_CorpCredits(object sender, RoutedEventArgs e)
        {
            WritePartCollectionToView(Client.Statuses.Corporative, true, Score.ScoreTypes.Credit);
        }

        /// <summary>
        /// Button action for "Add client"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_AddClient(object sender, RoutedEventArgs e)
        {
            AddClientView addClientView = new AddClientView(_service);
            addClientView.ShowDialog();
        }

        /// <summary>
        /// Event processing for save and view temporary data
        /// </summary>
        /// <param name="entity"></param>
        private void _service_SavedJsonObject(object entity)
        {
            WritePartCollectionToView();
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
        /// The focus of the line in the nameplate with the client's name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgClientsList_GotMouseCapture_Clients(object sender, MouseEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;

            if (dataGrid != null)
            {
                DataGridRow selectedRow = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem) as DataGridRow;

                if (selectedRow != null)
                {
                    Client client = selectedRow.Item as Client;

                    if (client != null)
                    {
                        _service.ClientId = client.Id;
                        WriteClientScoreToView<Client>(client.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Update client data from the table column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgClientsList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                Client editedClient = (Client)e.Row.Item;

                _service.Clients[(int)editedClient.Id - 1] = editedClient;
            }
            catch
            {
                MessageBox.Show("Something went wrong");
            }
        }

        /// <summary>
        /// The focus of the line in the nameplate with the score's name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgScoresList_GotMouseCapture_Scores(object sender, MouseEventArgs e)
        {
            this.Send_Money_Button.Visibility = Visibility.Visible;

            DataGrid dataGrid = sender as DataGrid;

            if (dataGrid != null)
            {
                DataGridRow selectedRow = dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem) as DataGridRow;

                if (selectedRow != null)
                {
                    Score score = selectedRow.Item as Score;

                    if (score != null)
                    {
                        _service.ScoreId = score.Id;
                        WriteClientScoreToView<Score>(score.Id);
                    }
                }
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

            CreateScore createScore = new CreateScore(_service);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    createScore.capitalizationCheckBox.Visibility = Visibility.Visible;
                    createScore.ShowDialog();

                    dgScoresList.Items.Refresh();
                    break;
                case MessageBoxResult.No:
                    createScore.capitalizationCheckBox.Visibility = Visibility.Hidden;
                    createScore.ShowDialog();

                    dgScoresList.Items.Refresh();
                    return;
            }
        }

        /// <summary>
        /// Action for the "Save to json" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_SaveJson(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                if (_service.JsonAddress != "")
                {
                    saveFileDialog.FileName = _service.JsonAddress;
                }

                saveFileDialog.DefaultExt = ".json";
                saveFileDialog.Filter = "JSON Files|*.json|All Files|*.*";
                saveFileDialog.Title = "Сохранить файл";

                if (saveFileDialog.ShowDialog() == true)
                {
                    _service.JsonAddress = saveFileDialog.FileName;
                }

                _service.SaveJsonWithAllData();
            }
            catch (JsonException)
            {
                MessageBox.Show("Something went wrong...");
            }
        }

        /// <summary>
        /// Action for the "Open json" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_OpenJson(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_service.JsonAddress == "")
                {
                    _service.JsonAddress = $@"{Environment.CurrentDirectory}";
                }

                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.DefaultExt = ".json";
                openFileDialog.Filter = "JSON Files|*.json|All Files|*.*";
                openFileDialog.Title = "Открыть файл";

                if (openFileDialog.ShowDialog() == true)
                {
                    _service.JsonAddress = openFileDialog.FileName;
                }

                _service.OpenJsonData();

                WritePartCollectionToView(null, false, null, true);

                Task.Run(() => { _service.CheckDeadline(); });
            }
            catch (JsonException)
            {
                MessageBox.Show("Something went wrong with open json...");
            }
        }

        /// <summary>
        /// Click for saving all data to the DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_SaveAllToDB(object sender, RoutedEventArgs e)
        {
            try
            {
                _service.SaveAllDataToDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//TODO
            }
        }

        /// <summary>
        /// Click for load all data from the DB.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_LoadAllFromDB(object sender, RoutedEventArgs e)
        {
            try
            {
                _service.LoadAllClientsFromDB();
                _service.LoadAllScoresFromDB();

                WritePartCollectionToView(null, false, null, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//TODO
            }
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            try
            {
                _service.Clients.Clear();
                _service.Scores.Clear();
                _service.ClientId = 0;
                _service.ScoreId = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);//TODO
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
                _service.CheckDeadline();
            }
            catch (ScoreException)
            {
                MessageBox.Show("Something went wrong...");
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
            ChoosingRecipient choosingRecipient = new ChoosingRecipient(_service);

            choosingRecipient.Show();

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
            dgScoresList.ItemsSource = null;
            dgClientsList.ItemsSource = null;

            if (status == null)
            {
                dgClientsList.ItemsSource = _service.Clients;
            }
            else
            {
                List<Client> generalClients = _service.Clients.Where(parameter => parameter.Status == status).ToList();
                dgClientsList.ItemsSource = new ObservableCollection<Client>(generalClients);
            }

            if (isScore)
            {
                if (typeScore.HasValue)
                {
                    List<Score> scoresGeneralPeoples = _service.Scores.Where(parameter
                        => _service.Clients[(int)parameter.ClientId - 1].Status == status
                        && parameter.ScoreType == typeScore).ToList();
                        dgScoresList.ItemsSource = new ObservableCollection<Score>(scoresGeneralPeoples);
                }
                else
                {
                    List<Score> scoresGeneralPeoples = _service.Scores.Where(parameter 
                        => _service.Clients[(int)parameter.ClientId - 1].Status == status).ToList();
                        dgScoresList.ItemsSource = new ObservableCollection<Score>(scoresGeneralPeoples);
                }
            }
            if (isAllScores)
            {
                ObservableCollection<Score> allScores = _service.Scores;
                dgScoresList.ItemsSource = allScores;
            }
        }

        private void WriteClientScoreToView<T>(long id)
        {
            if (typeof(T) == typeof(Client))
            {
                List<Score> scores = _service.Scores.Where(parameter => parameter.ClientId == id).ToList();
                dgScoresList.ItemsSource = new ObservableCollection<Score>(scores);
            }
            else if (typeof(T) == typeof(Score))
            {
                List<Score> scores = _service.Scores.Where(parameter => parameter.Id == id).ToList();
                List<Client> clients = _service.Clients.Where(paremetr => paremetr.Id == scores[0].ClientId).ToList();
                dgClientsList.ItemsSource = new ObservableCollection<Client>(clients);
            }
        }

    }
}
