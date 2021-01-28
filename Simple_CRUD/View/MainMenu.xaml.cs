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
        public RelayCommand OpenEngineCommand { get; private set; }

        private User User { get; set; }

        public MainMenu()
        {
            InitializeComponent();
            DataContext = this;
            User = new User() { Name = "Гость", Approved = false };
            AuthMessage = $"Добро пожаловать в панель управления, {User.Name}!";
            OpenEngineCommand = new RelayCommand(OpenEngine);
            OpenGameCommand = new RelayCommand(OpenGame);
            this.Closed += MainWindow_Closed;
        }

        private void OpenEngine()
        {
            var engine = new EngineView();
            engine.ShowDialog();
        }

        private void OpenGame()
        {
            var game = new GameView();
            game.ShowDialog();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
