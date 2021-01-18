using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Логика взаимодействия для TableView.xaml
    /// </summary>
    public partial class TableView : Window
    {
        Presenter.Table table;
        public TableView(string name)
        {
            InitializeComponent();
            table = new Presenter.Table(name);
            Display();
        }

        private void Display()
        {

            TV.Columns.Add(new DataGridTextColumn());
            TV.Columns[0].Header = table.headers[0];
            TV.Columns.Add(new DataGridCheckBoxColumn());
        }
    }
}
