using Bank;

namespace WpfUI.Models
{
    internal class SendMoneyModel
    {
        public Score Score { get; set; }
        public Client Client { get; set; }

        public SendMoneyModel(Score score, Client client)
        {
            Score = score;
            Client = client;
        }

        public override string? ToString()
        {
            return $"{Client}: " +
                $"{Score.ScoreType} - " +
                $"{Score.Balance}";
        }
    }
}
