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
    /// Логика взаимодействия для PlaygroundView.xaml
    /// </summary>
    public partial class PlaygroundView : Window, IExtendedWindow
    {
        public ObservableCollection<Country> Countries { get; private set; }
        public ObservableCollection<Playground> Playgrounds { get; private set; }
        public ObservableCollection<Man> Employers { get; private set; }

        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand RequestCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private readonly Context context;
        private readonly AuthUser User;
        public PlaygroundView(AuthUser user)
        {
            User = user;
            context = new Context(user);
            Countries = new ObservableCollection<Country>();
            Playgrounds = new ObservableCollection<Playground>();
            Employers = new ObservableCollection<Man>();
            InsertCommand = new RelayCommand(InsertHandler);
            RequestCommand = new RelayCommand(RequestHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            
            InitializeComponent();
            UpdateTable();
            DataContext = this;

            if (!user.Approved)
            {
                Insert.IsEnabled = false;
                PlaygroundsDataGrid.IsReadOnly = true;
            }
        }

        private void CountryComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var country = (Country)comboBox.SelectedItem;
            if (country.Id == -1)
            {
                var newCountry = new Country
                {
                    Id = context.Countries.Count() == 0 ? 1 : context.Countries.Select(p => p.Id).Max() + 1,
                };
                var addWindow = new AddNewContextElement(this, context, newCountry, (int)Tables.Numbers.Country);
                addWindow.ShowDialog();
            }
        }

        private void ManComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var man = (Man)comboBox.SelectedItem;
            if (man.Id == -1)
            {
                var newMan = new Man
                {
                    Id = context.People.Count() == 0 ? 1 : context.People.Select(p => p.Id).Max() + 1,
                };
                var addWindow = new AddNewContextElement(this, context, newMan, (int)Tables.Numbers.Man);
                addWindow.ShowDialog();
            }
        }

        private void RequestHandler()
        {
            var request = new GeneralRequestView(this, "Название площадки", "Страна площадки");
            request.ShowDialog();
        }

        public void UpdateTable()
        {
            UpdateTable(null, null);
        }

        public void UpdateTable(string firstField, string secondField)
        {
            //Стандартный запрос
            firstField = firstField == null ? null : firstField.ToLower().Trim();
            secondField = secondField == null ? null : secondField.ToLower().Trim();

            var contextAllPlaygrounds = context.Playgrounds.Where(p =>
                (firstField == null || (firstField != null && p.Name.ToLower().Contains(firstField))) &&
                (secondField == null || (secondField != null && p.Country.Name.ToLower().Contains(secondField)))
            );
            //--Стандартный запрос
            Playgrounds.Clear();
            Countries.Clear();
            Employers.Clear();
            foreach (var playground in contextAllPlaygrounds)
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

            Employers.Add(new Man
            {
                Id = -1,
                Name = "Другой..."
            });

            Countries.Add(new Country
            {
                Id = -1,
                Name = "Другая..."
            });



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
            if(User.Approved)
            {
                Playground playground = new Playground
                {
                    Id = Playgrounds.Count == 0 ? 1 :Playgrounds.Select(p => p.Id).Max() + 1,
                    CountryId = Countries.Select(c => c.Id).Min(),
                    EmployerId = Employers.Select(e => e.Id).Min(),
                    Name = "STEAM-" + (Playgrounds.Select(p => p.Id).Max() + 1)
                };

                playground.Country = Countries.First(c => c.Id == playground.CountryId);
                playground.Employer = Employers.First(c => c.Id == playground.EmployerId);

                
                context.Playgrounds.Add(playground);
                if(context.SaveChanges() != -1)
                {
                    Playgrounds.Add(playground);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (User.Approved)
            {
                Button but = (Button)sender;
                int id = int.Parse(but.Uid);
                var playground = Playgrounds.First(c => c.Id == id);
                context.Playgrounds.Remove(playground);
                if (context.SaveChanges() != -1)
                {
                    Playgrounds.Remove(playground);
                }
                
            }
        }

        private void PlaygroundsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if(User.Approved)
            {
                DataGrid dg = sender as DataGrid;

                Playground playground = context.Playgrounds.FirstOrDefault(e => e.Id == ((Playground)dg.SelectedItems[0]).Id);
                if (playground != null)
                {
                    context.Playgrounds.Update(playground);
                    context.SaveChanges();
                }
            }
        }
    }
}
