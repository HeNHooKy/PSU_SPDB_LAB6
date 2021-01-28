using Microsoft.EntityFrameworkCore;
using Npgsql;
using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
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
    public partial class EngineView : Window
    {
        //public ObservableCollection<Engine> Engines { get; private set; }
        //public ObservableCollection<Studio> Studios { get; private set; }
        public RelayCommand InsertCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand RejectCommand { get; private set; }

        private readonly Context context;
        
        
        public EngineView()
        {
            this.context = new Context();

            Engines = new ObservableCollection<Engine>();
            Studios = new ObservableCollection<Studio>();
            InsertCommand = new RelayCommand(InsertHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            
            InitializeComponent();
            UpdateTable();
            DataContext = this;
        }

        private void UpdateTable()
        {
            Engines.Clear();
            foreach (var studio in context.Studios)
            {
                Studios.Add(studio);
            }
            foreach (var engine in context.Engines)
            {
                Engines.Add(engine);
            }
            OnPropertyChanged(nameof(Engines));
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
            Engine engine = new Engine
            {
                Id = Engines.Select(e => e.Id).Max() + 1,
                ProductionYear = DateTime.Now,
                StudioId = Studios.Select(s => s.Id).Min()
            };
            engine.Studio = Studios.First(s => s.Id == engine.StudioId);
            engine.Name = $"X{engine.Id}";
            Engines.Add(engine);
            context.Engines.Add(engine);
            context.SaveChanges();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            int id = int.Parse(but.Uid);
            var engine = Engines.First(c => c.Id == id);
            Engines.Remove(engine);
            context.Engines.Remove(engine);
            context.SaveChanges();
        }

        private void EnginesDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            Engine engine = context.Engines.FirstOrDefault(e => e.Id == ((Engine)dg.SelectedItems[0]).Id);
            if(engine != null)
            {
                context.Engines.Update(engine);
                context.SaveChanges();
            }
        }
    }
}
