using MongoDB.Bson;
using MVVMlab.Models;
using MVVMlab.Services;
using MVVMlab.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace MVVMlab.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HttpDataService _httpDataService;
        private ObservableCollection<Item> _items;
        private Item _selectedItem;
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged("Items");
                }
            }
        }

        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                    (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand ShowDetailsCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public MainViewModel()
        {
            ShowDetailsCommand = new RelayCommand(async () => ShowDetails());
            EditCommand = new RelayCommand(EditItem, () => SelectedItem != null);
            AddCommand = new RelayCommand(AddProduct);
            DeleteCommand = new RelayCommand(DeleteProduct);
            Items = new ObservableCollection<Item>();
            _httpDataService = new HttpDataService();
            Task.Run(() => LoadItems());
        }

        private async Task LoadItems()
        {
            var json = await _httpDataService.GetDataAsync();

            try
            {
                var itemsArray = JsonConvert.DeserializeObject<Item[]>(json);
                var itemsCollection = new ObservableCollection<Item>(itemsArray);
                Items = itemsCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się pobrać danych z serwera. Zresetuj aplikację i sprawdź połączenie z serwerem.", "Błąd połączenia z serwerem", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine("Błąd deserializacji JSON-a: " + ex.Message);
            }
        }

        private void EditItem()
        {
            if (SelectedItem != null)
            {
                EditItemWindow editWindow = new EditItemWindow();
                EditItemViewModel editVM = new EditItemViewModel(SelectedItem, editWindow);
                editWindow.DataContext = editVM;
                editWindow.ShowDialog();

                Task.Run(() => LoadItems());
            }
        }
        public async void DeleteProduct()
        {
            if (SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten produkt?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var productId = SelectedItem.Id;
                    var deleteResult = await _httpDataService.DeleteProductAsync(productId);
                    if (deleteResult != null)
                    {
                        await LoadItems();
                    }
                    else
                    {
                        MessageBox.Show("Wystąpił błąd podczas usuwania produktu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano żadnego produktu do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ShowDetails()
        {
            if (SelectedItem != null)
            {
                DetailsWindow detailsWindow = new DetailsWindow();
                detailsWindow.DataContext = SelectedItem;
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nie wybrano żadnego produktu do wyświetlenia szczegółów.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void AddProduct()
        {
            AddItemWindow addWindow = new AddItemWindow();
            AddItemViewModel addVM = new AddItemViewModel(addWindow);
            addWindow.DataContext = addVM;
            addWindow.ShowDialog();

            Task.Run(() => LoadItems());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
