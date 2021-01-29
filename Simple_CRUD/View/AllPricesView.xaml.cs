using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
using Simple_CRUD.Tools;
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
    public partial class AllPricesView : Window
    {
        public ObservableCollection<Game> Games { get; private set; }
        public ObservableCollection<Playground> Playgrounds { get; private set; }
        public ObservableCollection<Price> AllPrices { get; private set; }

        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private readonly Context context;
        private readonly User User;

        public AllPricesView(User user)
        {
            Games = new ObservableCollection<Game>();
            Playgrounds = new ObservableCollection<Playground>();
            AllPrices = new ObservableCollection<Price>();

            User = user;
            context = new Context(user);
            InitializeComponent();
            InsertCommand = new RelayCommand(InsertHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            UpdateTable();

            if(!user.Approved)
            {
                Insert.IsEnabled = false;
                AllPricesDataGrid.IsReadOnly = true;
            }
        }

        private void UpdateTable()
        {
            AllPrices.Clear();
            foreach (var playground in context.Playgrounds)
            {
                Playgrounds.Add(playground);
            }
            foreach (var game in context.Games)
            {
                Games.Add(game);
            }
            foreach(var price in context.AllPrices)
            {
                AllPrices.Add(price);
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

            var ids = GetGamePlaygroundMinSet();
            Price price = new Price
            {
                Id = AllPrices.Select(p => p.Id).Max() + 1,
                GameId = ids.gameId,
                PlaygroundId = ids.playgroundId,
                Game = Games.First(g => g.Id == ids.gameId),
                Playground = Playgrounds.First(p => p.Id == ids.playgroundId),
                Cost = 0
            };
            
            AllPrices.Add(price);
            context.AllPrices.Add(price);
            context.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(User.Approved)
            {
                Button but = (Button)sender;
                int id = int.Parse(but.Uid);
                var price = AllPrices.First(c => c.Id == id);
                AllPrices.Remove(price);
                context.AllPrices.Remove(price);
                context.SaveChanges();
            }
        }

        private void AllPricessDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            Price price = context.AllPrices.FirstOrDefault(e => e.Id == ((Engine)dg.SelectedItems[0]).Id);
            if (price != null)
            {
                context.AllPrices.Update(price);
                context.SaveChanges();
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
