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
    /// Логика взаимодействия для ManView.xaml
    /// </summary>
    public partial class ManView : Window
    {
        public ObservableCollection<Man> People { get; private set; }
        public ObservableCollection<Country> Countries { get; private set; }

        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private readonly Context context;
        private readonly User User;
        public ManView(User user)
        {
            People = new ObservableCollection<Man>();
            Countries = new ObservableCollection<Country>();
            User = user;
            context = new Context(user);
            InitializeComponent();
            InsertCommand = new RelayCommand(InsertHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            UpdateTable();

            if (!user.Approved)
            {
                Insert.IsEnabled = false;
                PeopleDataGrid.IsReadOnly = true;
            }
        }

        private void UpdateTable()
        {
            People.Clear();
            foreach (var man in context.People)
            {
                People.Add(man);
            }
            foreach (var country in context.Countries)
            {
                Countries.Add(country);
            }
            OnPropertyChanged(nameof(People));
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
            Man man = new Man
            {
                Id = People.Select(m => m.Id).Max() + 1,
                Name = "John",
                Surname = "G.",
                CountryBornId = Countries.Select(c => c.Id).Min()
            };
            man.CountryBorn = Countries.First(c => c.Id == man.CountryBornId);

            People.Add(man);
            context.People.Add(man);
            context.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (User.Approved)
            {
                Button but = (Button)sender;
                int id = int.Parse(but.Uid);
                var man = People.First(c => c.Id == id);
                People.Remove(man);
                context.People.Remove(man);
                context.SaveChanges();
            }
        }


        private void PeopleDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            Man man = context.People.FirstOrDefault(e => e.Id == ((Engine)dg.SelectedItems[0]).Id);
            if (man != null)
            {
                context.People.Update(man);
                context.SaveChanges();
            }
        }
    }
}
