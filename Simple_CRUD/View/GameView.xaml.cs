using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
using Simple_CRUD.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simple_CRUD.View
{
    /// <summary>
    /// Логика взаимодействия для ManView.xaml
    /// </summary>
    public partial class GameView : Window
    {
        public ObservableCollection<Game> Games { get; private set; }
        public ObservableCollection<Publisher> Publishers { get; private set; }
        public ObservableCollection<Engine> Engines { get; private set; }
        public ObservableCollection<Studio> Studios { get; private set; }
        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand RejectCommand { get; private set; }

        private readonly Context context;
        private readonly User User;

        public GameView(User user)
        {
            this.context = new Context(user);
            Games = new ObservableCollection<Game>();
            Publishers = new ObservableCollection<Publisher>();
            Engines = new ObservableCollection<Engine>();
            Studios = new ObservableCollection<Studio>();
            InsertCommand = new RelayCommand(InsertHandler);
            CancelCommand = new RelayCommand(CancelHandler);

            InitializeComponent();
            UpdateTable();
            DataContext = this;

            if (!user.Approved)
            {
                Insert.IsEnabled = false;
                GamesDataGrid.IsReadOnly = true;
            }
        }

        private void UpdateTable()
        {
            Games.Clear();
            foreach (var game in context.Games)
            {
                Games.Add(game);
            }
            foreach(var publisher in context.Publishers)
            {
                Publishers.Add(publisher);
            }
            foreach(var studio in context.Studios)
            {
                Studios.Add(studio);
            }
            foreach(var engine in context.Engines)
            {
                Engines.Add(engine);
            }
            OnPropertyChanged(nameof(Games));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void CancelHandler()
        {
            Close();
        }

        private void InsertHandler()
        {
            Game game = new Game
            {
                Id = Games.Select(e => e.Id).Max() + 1,
                StudioId = Studios.Select(s => s.Id).Min(),
                EngineId = Engines.Select(e => e.Id).Min(),
                PublisherId = Publishers.Select(p => p.Id).Min(),
            };
            game.Studio = Studios.First(s => s.Id == game.StudioId);
            game.Engine = Engines.First(e => e.Id == game.EngineId);
            game.Publisher = Publishers.First(p => p.Id == game.PublisherId);
            game.Name = $"Assasin's Creed {game.Id}";
            Games.Add(game);
            context.Games.Add(game);
            context.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (User.Approved)
            {
                Button but = (Button)sender;
                int id = int.Parse(but.Uid);
                var game = Games.First(c => c.Id == id);
                Games.Remove(game);
                context.Games.Remove(game);
                context.SaveChanges();
            }
        }

        private void GamesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            Game game = context.Games.FirstOrDefault(e => e.Id == ((Game)dg.SelectedItems[0]).Id);
            if (game != null)
            {
                context.Games.Update(game);
                context.SaveChanges();
            }
        }
    }
}
