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
        public string AuthMessage { get; set; }
        public RelayCommand OpenGameCommand { get; private set; }
        public RelayCommand OpenPriceCommand { get; private set; }
        public RelayCommand OpenManCommand { get; private set; }
        public RelayCommand OpenPlaygrounCommand { get; private set; }

        private User User { get; set; }

        public MainMenu()
        {
            InitializeComponent();
            DataContext = this;
            User = new User() { Name = "Гость", Approved = false };
            AuthMessage = $"Добро пожаловать в панель управления, {User.Name}!";
            OpenPriceCommand = new RelayCommand(OpenPrice);
            OpenGameCommand = new RelayCommand(OpenGame);
            OpenManCommand = new RelayCommand(OpenMan);
            OpenPlaygrounCommand = new RelayCommand(OpenPlayground);
            this.Closed += MainWindow_Closed;
        }

        private void OpenPrice()
        {
            var price = new AllPricesView(User);
            price.ShowDialog();
        }

        private void OpenGame()
        {
            var game = new GameView(User);
            game.ShowDialog();
        }

        private void OpenMan()
        {
            var man = new ManView(User);
            man.ShowDialog();
        }

        private void OpenPlayground()
        {
            var playground = new PlaygroundView(User);
            playground.ShowDialog();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
