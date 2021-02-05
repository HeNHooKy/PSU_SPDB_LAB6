using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
using Simple_CRUD.Tools;
using Simple_CRUD.View.Support;
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
    public partial class GameView : Window, IExtendedWindow
    {
        public ObservableCollection<Game> Games { get; private set; }
        public ObservableCollection<Publisher> Publishers { get; private set; }
        public ObservableCollection<Engine> Engines { get; private set; }
        public ObservableCollection<Studio> Studios { get; private set; }
        public RelayCommand InsertCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand RequestCommand { get; private set; }

        private readonly Context context;
        private readonly AuthUser User;

        public GameView(AuthUser user)
        {
            this.context = new Context(user);
            User = user;
            Games = new ObservableCollection<Game>();
            Publishers = new ObservableCollection<Publisher>();
            Engines = new ObservableCollection<Engine>();
            Studios = new ObservableCollection<Studio>();
            InsertCommand = new RelayCommand(InsertHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            RequestCommand = new RelayCommand(RequestHandler);

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
            UpdateTable(null, null);
        }

        private void RequestHandler()
        {
            var request = new GeneralRequestView(this, "Название игры", "Студия разработчик");
            request.ShowDialog();
        }

        public void UpdateTable(string firstField, string secondField)
        {
            //Стандартный запрос
            firstField = firstField == null ? null : firstField.ToLower().Trim();
            secondField = secondField == null ? null : secondField.ToLower().Trim();

            var contextAllPrices = context.Games.Where(g =>
                (firstField == null || (firstField != null && g.Name.ToLower().Contains(firstField))) &&
                (secondField == null || (secondField != null && g.Studio.Name.ToLower().Contains(secondField)))
            );
            //--Стандартный запрос

            Games.Clear();
            Publishers.Clear();
            Studios.Clear();
            Engines.Clear();

            foreach (var game in contextAllPrices)
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
            if(User.Approved)
            {
                Game game = new Game
                {
                    Id = Games.Count == 0 ? 1 : Games.Select(e => e.Id).Max() + 1,
                    StudioId = Studios.Select(s => s.Id).Min(),
                    EngineId = Engines.Select(e => e.Id).Min(),
                    PublisherId = Publishers.Select(p => p.Id).Min(),
                };
                game.Studio = Studios.First(s => s.Id == game.StudioId);
                game.Engine = Engines.First(e => e.Id == game.EngineId);
                game.Publisher = Publishers.First(p => p.Id == game.PublisherId);
                game.Name = $"Assasin's Creed {game.Id}";
                
                context.Games.Add(game);
                if(context.SaveChanges() != -1)
                {
                    Games.Add(game);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (User.Approved)
            {
                Button but = (Button)sender;
                int id = int.Parse(but.Uid);
                var game = Games.First(c => c.Id == id);

                context.Games.Remove(game);
                if(context.SaveChanges() != -1)
                {
                    Games.Remove(game);
                }
            }
        }

        private void GamesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if(User.Approved)
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
}
