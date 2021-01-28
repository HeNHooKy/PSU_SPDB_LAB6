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
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand RejectCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }

        private readonly List<Row> DeleteQueue = new List<Row>();
        private readonly List<Man> InsertQueue = new List<Man>();
        private readonly List<Man> UpdateQueue = new List<Man>();

        private readonly IManProvider provider;

        public ManView(Query query)
        {
            People = new ObservableCollection<Man>();
            SaveCommand = new RelayCommand(SaveHandler);
            InsertCommand = new RelayCommand(InsertHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            UpdateCommand = new RelayCommand(UpdateTable);

            InitializeComponent();
            provider = new ManProvider(query, new CountryProvider(query));
            UpdateTable();
            DataContext = this;
        }

        private void UpdateTable()
        {
            People.Clear();
            foreach (var m in provider.Select())
            {
                People.Add(m);
            }
            OnPropertyChanged(nameof(People));
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
            Man man = new Man
            {
                Id = People.Select(m => m.Id).Max() + 1,
                CountryBorn = new Engine()
            };
            People.Add(man);
            InsertQueue.Add(man);
        }

        private void PeopleDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            Man man = (Man)dg.SelectedItems[0];
            if(InsertQueue.Contains(man))
            {
                return;
            }

            if (UpdateQueue.Contains(man))
            {
                UpdateQueue.Remove(man);
            }
            UpdateQueue.Add(man);
            Save.IsEnabled = true;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Save.IsEnabled = true;
            Button but = (Button)sender;
            int id = int.Parse(but.Uid);
            var man = People.FirstOrDefault(c => c.Id == id);
            People.Remove(man);
            DeleteQueue.Add(man);
            InsertQueue.Remove(man);
            UpdateQueue.Remove(man);
        }

        private void DeleteExecute()
        {
            foreach (Row row in DeleteQueue)
            {
                if (!provider.Delete((Man)row))
                {
                    OccurredProblem();
                    return;
                }
            }
            DeleteQueue.Clear();
        }

        private void InsertExecute()
        {
            foreach (var man in InsertQueue)
            {
                if (!provider.Insert(man))
                {
                    OccurredProblem();
                    return;
                }
            }
            InsertQueue.Clear();
        }

        private void UpdateExecute()
        {
            foreach (var man in UpdateQueue)
            {
                if (!provider.Update(man))
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
