using System;

namespace Bank
{
    public class Score : MyNotification
    {
        /// <summary>
        /// Score's ID
        /// </summary>
        private long _id;

        /// <summary>
        /// Score's balance
        /// </summary>
        private Decimal _balance;

        /// <summary>
        /// Interest on the account
        /// </summary>
        private Decimal _persents;

        /// <summary>
        /// Score's date
        /// </summary>
        private DateTime _dateScore;

        /// <summary>
        /// With сapitalization
        /// </summary>
        private bool _isСapitalization;

        /// <summary>
        /// Can you give out money
        /// </summary>
        private bool _isMoney;

        /// <summary>
        /// End date of the contract
        /// </summary>
        private DateTime _deadline;

        /// <summary>
        /// Date of the last dividend payment
        /// </summary>
        private DateTime _dateLastDividends;

        /// <summary>
        /// Owner
        /// </summary>
        private Client _client;

        /// <summary>
        /// Type score
        /// </summary>
        private ScoreTypes _scoreType;

        /// <summary>
        /// Score state
        /// </summary>
        private bool _isActive;

        public Score(ScoreTypes scoreType)
        {
            ScoreType = scoreType;
        }

        public ScoreTypes ScoreType
        {
            get => _scoreType;
            set
            {
                if (_scoreType == value)
                    return;
                this._scoreType = value;
                OnPropertyChanged(nameof(ScoreType));
            }
        }

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

        public Decimal Balance
        {
            get => _balance;
            set
            {
                if (_balance == value)
                    return;
                this._balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        public Decimal Persents
        {
            get => _persents;
            set
            {
                if (_persents == value)
                    return;
                this._persents = value;
                OnPropertyChanged(nameof(Persents));
            }
        }

        public DateTime DateScore
        {
            get => _dateScore;
            set
            {
                if (_dateScore == value)
                    return;
                OnPropertyChanged(nameof(DateScore));
            }
        }

        public bool IsСapitalization
        {
            get => _isСapitalization;
            set
            {
                if (_isСapitalization == value)
                    return;
                OnPropertyChanged(nameof(IsСapitalization));
            }
        }

        public bool IsMoney
        {
            get => _isMoney;
            set
            {
                if (_isMoney == value)
                    return;
                OnPropertyChanged(nameof(IsMoney));
            }
        }

        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                if (_deadline == value)
                    return;
                this._deadline = value;
                OnPropertyChanged(nameof(Deadline));
            }
        }

        public DateTime DateLastDividends
        {
            get => _dateLastDividends;
            set
            {
                if (_dateLastDividends == value)
                    return;
                this._dateLastDividends = value;
                OnPropertyChanged(nameof(DateLastDividends));
            }
        }

        public Client Client
        {
            get => _client;
            set
            {
                if (_client == value)
                    return;
                this._client = value;
                OnPropertyChanged(nameof(Client));
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value)
                    return;
                this._isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        public enum ScoreTypes
        {
            Credit, Deposit
        }

    }
}
