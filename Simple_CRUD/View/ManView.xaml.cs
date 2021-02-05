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
    /// Логика взаимодействия для ManView.xaml
    /// </summary>
    public partial class ManView : Window, IExtendedWindow
    {
        public ObservableCollection<Man> People { get; private set; }
        public ObservableCollection<Country> Countries { get; private set; }

        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand RequestCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private readonly Context context;
        private readonly AuthUser User;
        public ManView(AuthUser user)
        {
            User = user;
            context = new Context(user);
            People = new ObservableCollection<Man>();
            Countries = new ObservableCollection<Country>();
            InsertCommand = new RelayCommand(InsertHandler);
            RequestCommand = new RelayCommand(RequestHandler);
            CancelCommand = new RelayCommand(CancelHandler);

            InitializeComponent();
            UpdateTable();
            DataContext = this;

            if (!user.Approved)
            {
                Insert.IsEnabled = false;
                PeopleDataGrid.IsReadOnly = true;
            }
        }

        private void RequestHandler()
        {
            var request = new GeneralRequestView(this, "Имя", "Фамилия");
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

            var contextAllPrices = context.People.Where(p =>
                (firstField == null || (firstField != null && p.Name.ToLower().Contains(firstField))) &&
                (secondField == null || (secondField != null && p.Surname.ToLower().Contains(secondField)))
            );
            //--Стандартный запрос
            People.Clear();
            Countries.Clear();
            foreach (var country in context.Countries)
            {
                Countries.Add(country);
            }
            foreach (var man in context.People)
            {
                People.Add(man);
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
            if(User.Approved)
            {
                Man man = new Man
                {
                    Id = People.Count == 0 ? 1 : People.Select(m => m.Id).Max() + 1,
                    Name = "John",
                    Surname = "G." + (People.Select(m => m.Id).Max() + 1),
                    CountryBornId = Countries.Select(c => c.Id).Min()
                };
                man.CountryBorn = Countries.First(c => c.Id == man.CountryBornId);

                
                context.People.Add(man);
                if(context.SaveChanges() != -1)
                {
                    People.Add(man);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (User.Approved)
            {
                Button but = (Button)sender;
                int id = int.Parse(but.Uid);
                var man = People.First(c => c.Id == id);
                
                context.People.Remove(man);
                if(context.SaveChanges() != -1)
                {
                    People.Remove(man);
                }
            }
        }


        private void PeopleDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if(User.Approved)
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
}
