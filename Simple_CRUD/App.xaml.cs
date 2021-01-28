using Microsoft.EntityFrameworkCore;
using Simple_CRUD.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_CRUD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception.GetType().Equals(typeof(DbUpdateException)))
            {
                MessageBox.Show($"Критическая ошибка 'Невозможно удалить связанную запись': {e.Exception.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
            else
            {
                MessageBox.Show($"An unhandled exception just occurred: " + e.Exception.Message + "\nStackTrace:" + e.Exception.StackTrace, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Handled = true;
            }
        }
    }
}
