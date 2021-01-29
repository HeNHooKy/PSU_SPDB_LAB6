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
    /// Логика взаимодействия для PlaygroundView.xaml
    /// </summary>
    public partial class PlaygroundView : Window
    {
        public ObservableCollection<Country> Countries { get; private set; }
        public ObservableCollection<Playground> Playgrounds { get; private set; }
        public ObservableCollection<Man> Employers { get; private set; }

        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private readonly Context context;
        private readonly User User;
        public PlaygroundView(User user)
        {
            Countries = new ObservableCollection<Country>();
            Playgrounds = new ObservableCollection<Playground>();
            Employers = new ObservableCollection<Man>();
            User = user;
            context = new Context(user);
            InitializeComponent();
            InsertCommand = new RelayCommand(InsertHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            UpdateTable();

            if (!user.Approved)
            {
                Insert.IsEnabled = false;
                PlaygroundsDataGrid.IsReadOnly = true;
            }
        }

        private void UpdateTable()
        {
            Playgrounds.Clear();
            foreach (var playground in context.Playgrounds)
            {
                Playgrounds.Add(playground);
            }
            foreach (var country in context.Countries)
            {
                Countries.Add(country);
            }
            foreach(var man in context.People)
            {
                Employers.Add(man);
            }
            OnPropertyChanged(nameof(Playgrounds));
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

            Playground playground = new Playground
            {
                Id = Playgrounds.Select(p => p.Id).Max() + 1,
                CountryId = Countries.Select(c => c.Id).Min(),
                EmployerId = Employers.Select(e => e.Id).Min(),
                Name = "STEAM"
            };

            playground.Country = Countries.First(c => c.Id == playground.CountryId);
            playground.Employer = Employers.First(c => c.Id == playground.EmployerId);

            Playgrounds.Add(playground);
            context.Playgrounds.Add(playground);
            context.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (User.Approved)
            {
                Button but = (Button)sender;
                int id = int.Parse(but.Uid);
                var playground = Playgrounds.First(c => c.Id == id);
                Playgrounds.Remove(playground);
                context.Playgrounds.Remove(playground);
                context.SaveChanges();
            }
        }

        private void PlaygroundsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            Playground playground = context.Playgrounds.FirstOrDefault(e => e.Id == ((Engine)dg.SelectedItems[0]).Id);
            if (playground != null)
            {
                context.Playgrounds.Update(playground);
                context.SaveChanges();
            }
        }
    }
}
