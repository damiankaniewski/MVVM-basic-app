using GalaSoft.MvvmLight.Command;
using MVVMlab.Models;
using MVVMlab.Services;
using MVVMlab.Validators;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MVVMlab.ViewModels
{
    internal class AddItemViewModel
    {
        private readonly HttpDataService _httpDataService;
        private readonly INotificationService _notificationService;
        private readonly Validator _validator;
        private readonly Window _window;
        public ICommand AddCommand { get; private set; }

        private NewItem _newItem;
        public NewItem NewItem
        {
            get { return _newItem; }
            set
            {
                _newItem = value;
                OnPropertyChanged(nameof(NewItem));
            }
        }
        public AddItemViewModel(Window window)
        {
            _httpDataService = new HttpDataService();
            _notificationService = new NotificationService();
            _validator = new Validator();
            _window = window;
            NewItem = new NewItem();
            AddCommand = new RelayCommand(AddItem);
        }

        private async void AddItem()
        {
            var validationErrors = _validator.ValidateNewItem(NewItem);

            if (validationErrors.Count > 0)
            {
                var errorMessage = string.Join("\n", validationErrors);
                await _notificationService.ShowMessage(errorMessage, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string json = JsonConvert.SerializeObject(NewItem);

            string result = await _httpDataService.AddProductAsync(json);
            if (result == null)
            {
                await _notificationService.ShowMessage("Dodawanie produktu nie powiodło się.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                await _notificationService.ShowMessage("Produkt został dodany pomyślnie.", "Operacja ukończona", MessageBoxButton.OK, MessageBoxImage.Error);
                NewItem = new NewItem();
                _window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
