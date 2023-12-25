using Bank.Buisness;
using MarshalsExceptions;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для EntDeadline.xaml
    /// </summary>
    public partial class CreateScore : Window
    {
        private readonly Service _service;
        private readonly NLog.Logger _logger;

        public CreateScore(Service service, NLog.Logger logger)
        {
            InitializeComponent();

            _service = service;
            _logger = logger;
        }
        /// <summary>
        /// Period
        /// </summary>
        private int _period = 0;

        /// <summary>
        /// Sum
        /// </summary>
        private Decimal _sum = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (entPeriod != null
                    && int.TryParse($"{entPeriod.Text}", out _period)
                    && Decimal.TryParse($"{entSum.Text}", out _sum))
                {
                    _period = int.Parse($"{entPeriod.Text}");
                    _sum = Decimal.Parse($"{entSum.Text}");

                    //This means that we are working with a deposit
                    if (capitalizationCheckBox.Visibility == Visibility.Visible)
                    {
                        if (capitalizationCheckBox.IsChecked == true)
                        {
                            _service.CreateNewScore(Score.ScoreTypes.Deposit, true, _period, _sum);
                        }
                        else
                        {
                            _service.CreateNewScore(Score.ScoreTypes.Deposit, false, _period, _sum);
                        }
                    }
                    //This means that we are working with a credit
                    else
                    {
                        _service.CreateNewScore(Score.ScoreTypes.Credit, false, _period, _sum);
                    }
                }
                else
                {
                    _logger.Error("Enter the number (not a word)");
                    MessageBox.Show("Enter the number (not a word)");
                }
            }
            catch (ScoreException ex)
            {
                _logger.Error("Something went wrong: " + ex.Message);
                MessageBox.Show("Something went wrong: " + ex.Message);
            }

            Close();
        }

    }
}
