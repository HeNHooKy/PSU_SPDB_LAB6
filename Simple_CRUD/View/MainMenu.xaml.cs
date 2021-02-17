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
        public RelayCommand OpenGameCommand { get; private set; }
        public RelayCommand OpenPriceCommand { get; private set; }
        public RelayCommand OpenManCommand { get; private set; }
        public RelayCommand OpenPlaygrounCommand { get; private set; }
        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand RegistrationCommand { get; private set; }

        private AuthUser User { get; set; }

        public MainMenu()
        {
            InitializeComponent();
            DataContext = this;
            User = new AuthUser() { Name = "Гость", Approved = true };
            AuthNewUser();
            OpenPriceCommand = new RelayCommand(OpenPrice);
            OpenGameCommand = new RelayCommand(OpenGame);
            OpenManCommand = new RelayCommand(OpenMan);
            OpenPlaygrounCommand = new RelayCommand(OpenPlayground);
            LoginCommand = new RelayCommand(OpenLogin);
            RegistrationCommand = new RelayCommand(OpenRegisetration);
            this.Closed += MainWindow_Closed;
        }

        public void AuthNewUser()
        {
            AuthMessage.Text = $"Добро пожаловать в панель управления, {User.Name}!";
        }

        public void CloseOpened(Window window)
        {
            window.Close();
        }

        private void OpenPrice()
        {
            var price = new AllPricesView(User);
            price.ShowDialog();
        }

        private void OpenLogin()
        {
            var login = new Login(this, User);
            login.ShowDialog();
        }

        private void OpenRegisetration()
        {
            var registration = new Registration(User);
            registration.ShowDialog();
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
