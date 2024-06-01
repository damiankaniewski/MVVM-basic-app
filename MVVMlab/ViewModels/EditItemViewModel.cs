using GalaSoft.MvvmLight.Command;
using MVVMlab.Models;
using MVVMlab.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using MongoDB.Bson;
using System.Windows;
using MVVMlab.Validators;

namespace MVVMlab.ViewModels
{
    public class EditItemViewModel : INotifyPropertyChanged
    {
        private readonly HttpDataService _httpDataService;
        private readonly INotificationService _notificationService;
        private readonly Validator _validator;
        private readonly Window _window;
        public Item SelectedItem { get; set; }

        public ICommand SaveEditCommand { get; private set; }

        public EditItemViewModel(Item item, Window window)
        {
            _httpDataService = new HttpDataService();
            _notificationService = new NotificationService();
            _validator = new Validator();
            _window = window;
            SelectedItem = item;
            SaveEditCommand = new RelayCommand(SaveEdit, CanExecuteSaveEdit);
        }

        private async void SaveEdit()
        {
            var validationErrors = _validator.ValidateItem(SelectedItem);

            if (validationErrors.Count > 0)
            {
                var errorMessage = string.Join("\n", validationErrors);
                MessageBox.Show(errorMessage, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string json = JsonConvert.SerializeObject(SelectedItem);
            ItemId productId = SelectedItem.Id; 

            string result = await _httpDataService.UpdateProductAsync(productId, json);
            if (result == null)
            {
                MessageBox.Show("Aktualizacja produktu nie powiodła się.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Aktualizacja produktu wykonała się pomyślnie.");
                OnPropertyChanged("SelectedItem");
                _window.Close();
            }
        }
        private bool CanExecuteSaveEdit()
        {
            return SelectedItem != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
