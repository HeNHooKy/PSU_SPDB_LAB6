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
        public RelayCommand OpenEngineCommand { get; private set; }

        public MainMenu()
        {
            InitializeComponent();
            DataContext = this;
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
