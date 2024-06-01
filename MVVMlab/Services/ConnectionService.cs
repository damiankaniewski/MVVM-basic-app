using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMlab.Services
{
    public class ConnectionService
    {
        private const int CheckIntervalSeconds = 5;

        public event EventHandler ConnectionLost;

        public ConnectionService()
        {
            // Rozpocznij monitorowanie połączenia w tle
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(CheckIntervalSeconds));
                    if (!await IsConnectedToServerAsync())
                    {
                        OnConnectionLost();
                    }
                }
            });
        }

        private async Task<bool> IsConnectedToServerAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://localhost:7296/api/products");
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        protected virtual void OnConnectionLost()
        {
            ConnectionLost?.Invoke(this, EventArgs.Empty);
        }
    }
}
