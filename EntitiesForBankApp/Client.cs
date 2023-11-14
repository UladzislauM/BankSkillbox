﻿using System.Numerics;

namespace Bank
{
    public class Client : MyNotification
    {
        /// <summary>
        /// Client's ID
        /// </summary>
        private long _id;

        /// <summary>
        /// Client's name
        /// </summary>
        private string _firstName;

        /// <summary>
        /// Client's last name
        /// </summary>
        private string _lastName;

        /// <summary>
        /// Credit history
        /// </summary>
        private double _history;

        /// <summary>
        /// Client's status
        /// </summary>
        private int _prestige;

        public enum Statuses
        {
            General, VIP, Corporative
        }

        public Client (Statuses status)
        {
            Status = status;
        }

        public Statuses Status { get; private set; }

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

        public double History
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

    }
}