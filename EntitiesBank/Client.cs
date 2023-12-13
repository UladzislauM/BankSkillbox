namespace Bank
{
    public class Client : MyNotification
    {
        private long _id;
        private string _firstName;
        private string _lastName;
        private string _history;
        private int _prestige;
        private Statuses _status;
        private List<Score> _scores;

        public Client(Statuses status)
        {
            Status = status;
        }

        /// <summary>
        /// Client's ID
        /// </summary>
        public long Id
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;
                this._id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// Client's name
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName == value)
                    return;
                this._firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>
        /// Client's last name
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value)
                    return;
                this._lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        /// <summary>
        /// Credit history
        /// </summary>
        public string History
        {
            get => _history;
            set
            {
                if (_history == value)
                    return;
                this._history = value;
                OnPropertyChanged(nameof(History));
            }
        }

        /// <summary>
        /// Client's prestige
        /// </summary>
        public int Prestige
        {
            get => _prestige;
            set
            {
                if (_prestige == value)
                    return;
                this._prestige = value;
                OnPropertyChanged(nameof(Prestige));
            }
        }

        /// <summary>
        /// Client's status
        /// </summary>
        public Statuses Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;
                this._status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        /// <summary>
        /// Client's scores
        /// </summary>
        public List<Score> Scores
        {
            get => _scores;
            set
            {
                if (_scores == value)
                    return;
                this._scores = value;
                OnPropertyChanged(nameof(Scores));
            }
        }

        public enum Statuses
        {
            General, VIP, Corporative
        }

    }
}
