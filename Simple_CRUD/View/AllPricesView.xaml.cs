using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
using Simple_CRUD.Tools;
using Simple_CRUD.View.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    /// Логика взаимодействия для AllPricesView.xaml
    /// </summary>
    public partial class AllPricesView : Window, IExtendedWindow
    {
        public ObservableCollection<Game> Games { get; private set; }
        public ObservableCollection<Playground> Playgrounds { get; private set; }
        public ObservableCollection<Price> AllPrices { get; private set; }

        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand RequestCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private readonly Context context;
        private readonly AuthUser User;

        public AllPricesView(AuthUser user)
        {
            User = user;
            context = new Context(user);
            Games = new ObservableCollection<Game>();
            Playgrounds = new ObservableCollection<Playground>();
            AllPrices = new ObservableCollection<Price>();
            InsertCommand = new RelayCommand(InsertHandler);
            RequestCommand = new RelayCommand(RequestHandler);
            CancelCommand = new RelayCommand(CancelHandler);

            InitializeComponent();
            UpdateTable();
            DataContext = this;

            if (!user.Approved)
            {
                Insert.IsEnabled = false;
                AllPricesDataGrid.IsReadOnly = true;
            }
        }

        private void RequestHandler()
        {
            var request = new GeneralRequestView(this, "Игра", "Игровая площадка");
            request.ShowDialog();
        }

        private void UpdateTable()
        {
            UpdateTable(null, null);
        }

        public void UpdateTable(string firstField, string secondField)
        {
            //Стандартный запрос
            firstField = firstField == null ? null : firstField.ToLower().Trim();
            secondField = secondField == null ? null : secondField.ToLower().Trim();

            var contextAllPrices = context.AllPrices.Where(ap =>
                (firstField == null || (firstField != null && ap.Game.Name.ToLower().Contains(firstField))) &&
                (secondField == null || (secondField != null && ap.Playground.Name.ToLower().Contains(secondField)))
            );
            //--Стандартный запрос

            AllPrices.Clear();
            Playgrounds.Clear();
            Games.Clear();
            

            foreach (var price in contextAllPrices)
            {
                AllPrices.Add(price);
            }
            foreach (var playground in context.Playgrounds)
            {
                Playgrounds.Add(playground);
            }
            foreach (var game in context.Games)
            {
                Games.Add(game);
            }
            OnPropertyChanged(nameof(AllPrices));
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
                var (gameId, playgroundId) = GetGamePlaygroundMinSet();

                Price price = new Price
                {
                    Id = context.AllPrices.Count() == 0 ? 1 : context.AllPrices.Select(p => p.Id).Max() + 1,
                    GameId = gameId,
                    PlaygroundId = playgroundId,
                    Game = Games.First(g => g.Id == gameId),
                    Playground = Playgrounds.First(p => p.Id == playgroundId),
                    Cost = 0
                };

                
                context.AllPrices.Add(price);
                if(context.SaveChanges() != -1)
                {
                    AllPrices.Add(price);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(User.Approved)
            {
                Button but = (Button)sender;
                int id = int.Parse(but.Uid);
                var price = AllPrices.First(c => c.Id == id);
                
                context.AllPrices.Remove(price);
                if(context.SaveChanges() != -1)
                {
                    AllPrices.Remove(price);
                }
            }
        }

        private void AllPricessDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if(User.Approved)
            {
                DataGrid dg = sender as DataGrid;

                Price price = context.AllPrices.FirstOrDefault(e => e.Id == ((Price)dg.SelectedItems[0]).Id);
                if (price != null)
                {
                    context.AllPrices.Update(price);
                    context.SaveChanges();
                }
            }
        }

        private (int gameId, int playgroundId) GetGamePlaygroundMinSet()
        {
            (int gameId, int playgroundId) result = (int.MaxValue, int.MaxValue);
            foreach(var game in Games)
            {
                foreach(var playground in Playgrounds)
                {
                    if (AllPrices.FirstOrDefault(p => p.GameId == game.Id && p.PlaygroundId == playground.Id) == null)
                    {
                        result.playgroundId = playground.Id;
                        result.gameId = game.Id;
                    }
                }
            }

            if(result.gameId == int.MaxValue && result.playgroundId == int.MaxValue)
            {
                throw new Exception("Все доступные иры уже выставлены на продажу на всех доступных площадках");
            }

            return result;
        }
    }
}
