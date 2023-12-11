using Bank.Buisness;
using MarshalsExceptions;
using System;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для EntDeadline.xaml
    /// </summary>
    public partial class CreateScore : Window
    {
        private readonly Service _service;

        public CreateScore(Service service)
        {
            InitializeComponent();

            _service = service;
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
                    MessageBox.Show("Введите число (не букву)");
                }
            }
            catch (ScoreException)
            {
                MessageBox.Show("Something went wrong...");
            }

            Close();
        }

    }
}
