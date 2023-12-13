﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank
{
    public class Score : MyNotification
    { 
        private long _id;
        private Decimal _balance;
        private Decimal _percent;
        private DateTime _dateScore;
        private bool _isСapitalization;
        private bool _isMoney;
        private DateTime _deadline;
        private DateTime _dateLastDividends;
        private Client _client;
        private long _clientId;
        private bool _isActive;
        private ScoreTypes _scoreType;

        public Score(ScoreTypes scoreType)
        {
            ScoreType = scoreType;
            _dateScore = DateTime.Now;
            _dateLastDividends = DateTime.Now;
        }

        /// <summary>
        /// Score's ID
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
        /// Score's balance
        /// </summary>
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


        /// <summary>
        /// Score's percent
        /// </summary>
        public Decimal Percent
        {
            get => _percent;
            set
            {
                if (_percent == value)
                    return;
                this._percent = value;
                OnPropertyChanged(nameof(Percent));
            }
        }

        /// <summary>
        /// Score's date
        /// </summary>
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

        /// <summary>
        /// Score's сapitalization
        /// </summary>
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

        /// <summary>
        /// Can you give out money
        /// </summary>
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

        /// <summary>
        /// End date of the score contract
        /// </summary>
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

        /// <summary>
        /// Date of the last dividend score payment
        /// </summary>
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

        /// <summary>
        /// Score's owner
        /// </summary>
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
        
        /// <summary>
        /// Score's owner id.
        /// </summary>
        public long ClientId
        {
            get => _clientId;
            set
            {
                if (_clientId == value)
                    return;
                this._clientId = value;
                OnPropertyChanged(nameof(Client));
            }
        }

        /// <summary>
        /// Score's work status
        /// </summary>
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

        /// <summary>
        /// Score type
        /// </summary>
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

        public enum ScoreTypes
        {
            Credit, Deposit
        }

    }
}