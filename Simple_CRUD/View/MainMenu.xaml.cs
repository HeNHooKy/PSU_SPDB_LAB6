using Microsoft.VisualBasic;
using Npgsql;
using Simple_CRUD.Model;
using Simple_CRUD.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simple_CRUD.View
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    /// 

    public partial class MainMenu : Window
    {
        public RelayCommand OpenFTCommand;
        public string FTName = "мебель";

        public MainMenu()
        {
            InitializeComponent();
            var tView = new TableView(FTName);
            tView.Show();
            OpenFTCommand = new RelayCommand(OpenFT);
            
        }
        
        private void OpenFT()
        {
            
            
        }

        




    }

    


}
