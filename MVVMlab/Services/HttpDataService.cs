using MongoDB.Bson;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MVVMlab.Services;
using MVVMlab.Models;

namespace MVVMlab.Services
{
    public class HttpDataService
    {
        private readonly ConnectionService _connectionService;
        private readonly INotificationService _notificationService;
        private readonly HttpClient _httpClient;
        private string _url;

        public HttpDataService()
        {
            _connectionService = new ConnectionService();
            _notificationService = new NotificationService();
            _connectionService.ConnectionLost += OnConnectionLost;
            _httpClient = new HttpClient();
            _url = "https://localhost:7296/api/products/";
        }

        public async Task<string> GetDataAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas pobierania danych: {ex.Message}");
                return null;
            }
        }

        public async Task<string> GetItemDataAsync(ItemId productId)
        {
            try
            {
                var objId = productId.ToObjectId();
                var url = $"{_url}{productId}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas pobierania danych: {ex.Message}");
                return null;
            }
        }

        public async Task<string> AddProductAsync(string json)
        {
            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas dodawania produktu: {ex.Message}");
                return null;
            }
        }

        public async Task<string> UpdateProductAsync(ItemId productId, string json)
        {
            try
            {
                var objId = productId.ToObjectId();
                var url = $"{_url}{objId}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas aktualizacji produktu: {ex.Message}");
                return null;
            }
        }

        public async Task<string> DeleteProductAsync(ItemId productId)
        {
            try
            {
                var objId = productId.ToObjectId();
                var url = $"{_url}{objId}";
                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas usuwania produktu: {ex.Message}");
                return null;
            }
        }

        private void OnConnectionLost(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                await _notificationService.ShowMessage("Utracono połączenie z serwerem!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }
}
