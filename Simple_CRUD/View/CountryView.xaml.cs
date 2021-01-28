using Npgsql;
using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
using Simple_CRUD.Provider;
using Simple_CRUD.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
    /// Логика взаимодействия для CountryView.xaml
    /// </summary>
    public partial class CountryView : Window
    {
        public ObservableCollection<Engine> Countries { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand RejectCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }

        private readonly List<Row> DeleteQueue = new List<Row>();
        private readonly List<Engine> InsertQueue = new List<Engine>();
        private readonly List<Engine> UpdateQueue = new List<Engine>();

        private readonly ICountryProvider provider;
        
        public CountryView(Query query)
        {
            Countries = new ObservableCollection<Engine>();
            SaveCommand = new RelayCommand(SaveHandler);
            InsertCommand = new RelayCommand(InsertHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            UpdateCommand = new RelayCommand(UpdateTable);

            InitializeComponent();
            provider = new CountryProvider(query);
            UpdateTable();
            DataContext = this;
        }

        private void UpdateTable()
        {
            Countries.Clear();
            foreach (var c in provider.Select())
            {
                Countries.Add(c);
            }
            OnPropertyChanged(nameof(Countries));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void SaveHandler()
        {
            Save.IsEnabled = false;
            InsertExecute();
            UpdateExecute();
            DeleteExecute();
            UpdateTable();
        }

        private void CancelHandler()
        {
            Close();
        }

        private void InsertHandler()
        {
            Save.IsEnabled = true;
            Engine country = new Engine
            {
                Id = Countries.Select(c => c.Id).Max() + 1
            };
            Countries.Add(country);
            InsertQueue.Add(country);
        }

        private void CountriesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            Engine country = (Engine)dg.SelectedItems[0];
            if(UpdateQueue.Contains(country))
            {
                UpdateQueue.Remove(country);
            }
            UpdateQueue.Add(country);
            Save.IsEnabled = true;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Save.IsEnabled = true;
            Button but = (Button)sender;
            int id = int.Parse(but.Uid);
            var country = Countries.FirstOrDefault(c => c.Id == id);
            Countries.Remove(country);
            DeleteQueue.Add(country);
            InsertQueue.Remove(country);
            UpdateQueue.Remove(country);
        }

        private void DeleteExecute()
        {
            foreach (Row row in DeleteQueue)
            {
                if(!provider.Delete((Engine)row))
                {
                    OccurredProblem();
                    return;
                }
            }
            DeleteQueue.Clear();
        }

        private void InsertExecute()
        {
            foreach(var country in InsertQueue)
            {
                if(!provider.Insert(country))
                {
                    OccurredProblem();
                    return;
                }
            }
            InsertQueue.Clear();
        }

        private void UpdateExecute()
        {
            foreach(var country in UpdateQueue)
            {
                if(!provider.Update(country))
                {
                    OccurredProblem();
                    return;
                }
            }
            UpdateQueue.Clear();
        }

        private void OccurredProblem()
        {
            DeleteQueue.Clear();
            InsertQueue.Clear();
            UpdateQueue.Clear();
            UpdateTable();
        }

        
    }
}
