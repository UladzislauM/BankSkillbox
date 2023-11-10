﻿using Bank.Buisness;
using Bank.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bank
{
    // Создать прототип банковской системы, позвляющей управлять клиентами и клиентскими счетами.
    // В информационной системе есть возможность перевода денежных средств между счетами пользователей
    // Открывать вклады, с капитализацией и без
    // * Продумать возможность выдачи кредитов
    // Продумать использование обобщений

    // Продемонстрировать работу созданной системы

    // Банк
    // ├── Отдел работы с обычными клиентами
    // ├── Отдел работы с VIP клиентами
    // └── Отдел работы с юридическими лицами

    // Дополнительно: клиентам с хорошей кредитной историей предлагать пониженую ставку по кредиту и 
    // повышенную ставку по вкладам
    // Добавить механизмы оповещений использую делегаты и события
    // Реализовать журнал действий, который будет хранить записи всех транзакций по 
    // счетам / вкладам / кредитам
    // 

    public partial class MainWindow : Window
    {
        /// <summary>
        /// Дата проверки. Для выдачи Суммы
        /// </summary>
        public DateTime dateTest;//TODO

        private readonly Service _service;

        public MainWindow()
        {
            InitializeComponent();
            dateTest = DateTime.Now;

            ILogger<Service> serviceLogger = new LoggerFactory().CreateLogger<Service>();
            _service = new Service(serviceLogger, new Repository());

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
            foreach(Score score in scores)
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
            catch
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

                Task.Run(() => { _service.CheckDeadline(); });
            }
            catch
            {
                MessageBox.Show("Something went wrong...");
                Close();
            }
        }

        /// <summary>
        /// Check deadline all scores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_TestDep(object sender, RoutedEventArgs e)
        {
            _service.CheckDeadline();
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

        private void WritePartCollectionToView(Client.Statuses? status = null, bool isScore = false, Score.ScoreTypes? typeScore = null)
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
                    List<Score> scoresGeneralPeoples = _service.Scores.Where(parameter => parameter.Client.Status == status
                    && parameter.ScoreType == typeScore).ToList();
                    dgScoresList.ItemsSource = new ObservableCollection<Score>(scoresGeneralPeoples);
                }
                else
                {
                    List<Score> scoresGeneralPeoples = _service.Scores.Where(parameter => parameter.Client.Status == status).ToList();
                    dgScoresList.ItemsSource = new ObservableCollection<Score>(scoresGeneralPeoples);
                }
            }
        }

        private void WriteClientScoreToView<T>(long id)
        {
            if (typeof(T) == typeof(Client))
            {
                List<Score> scores = _service.Scores.Where(parameter => parameter.Client.Id == id).ToList();
                dgScoresList.ItemsSource = new ObservableCollection<Score>(scores);
            }
            else
            {
                List<Score> scores = _service.Scores.Where(parameter => parameter.Id == id).ToList();
                List<Client> clients = _service.Clients.Where(paremetr => paremetr.Id == scores[0].Client.Id).ToList();
                dgClientsList.ItemsSource = new ObservableCollection<Client>(clients);
            }
        }

    }
}
