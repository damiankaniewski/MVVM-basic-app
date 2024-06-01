using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMlab.Services
{
    internal class NotificationService : INotificationService
    {
        public Task ShowMessage(string message, string title, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBox.Show(message, title, button, icon);
            return Task.CompletedTask;
        }
    }
}