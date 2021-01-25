using Simple_CRUD.Model;
using Simple_CRUD.Tools;
using System;
using System.Windows;

namespace Simple_CRUD.View
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private readonly Query query;
        public RelayCommand OpenCountryCommand { get; private set; }
        public RelayCommand OpenManCommand { get; private set; }

        public MainMenu()
        {
            InitializeComponent();
            query = new Query();
            DataContext = this;
            OpenCountryCommand = new RelayCommand(OpenCountry);
            OpenManCommand = new RelayCommand(OpenMan);
            this.Closed += MainWindow_Closed;
        }

        private void OpenMan()
        {
            var man = new ManView(query);
            man.ShowDialog();
        }

        private void OpenCountry()
        {
            var country = new CountryView(query);
            country.ShowDialog();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
