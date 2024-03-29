﻿using Bank;
using Bank.Buisness;
using WpfUI.ViewModel.Notifications;

namespace WpfUI.ViewModel
{
    abstract class CreateEntityViewModel<T> : DialogBlockViewModel
    {
        public List<T> Parameteres { get; set; }
        private T _parameter;

        public DefaultCommand CreateNewEntityCommand { get; }

        public CreateEntityViewModel(Service service) : base(service)
        {
            CreateNewEntityCommand = new DefaultCommand(ExecuteCreateNewEntity);
        }

        public T Parameter
        {
            get => _parameter;
            set
            {
                _parameter = value;
                OnPropertyChanged(nameof(Parameter));
            }
        }

        protected abstract void ExecuteCreateNewEntity(object? parameter);

    }
}
