using Simple_CRUD.Tools;
using Simple_CRUD.View.Support;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для GeneralRequestView.xaml
    /// </summary>
    public partial class GeneralRequestView : Window
    {
        public string FirstRequestName { get; set; }
        public string FirstRequestInput { get; set; }
        public string SecondRequestName { get; set; }
        public string SecondRequestInput { get; set; }

        public RelayCommand MakeRequestCommand { get; private set; }
        private IExtendedWindow callingWindow;

        public GeneralRequestView(IExtendedWindow window, string first, string second)
        {
            callingWindow = window;
            FirstRequestName = first;
            SecondRequestName = second;
            MakeRequestCommand = new RelayCommand(MakeRequestHandler);
            InitializeComponent();
            DataContext = this;
        }


        private void MakeRequestHandler()
        {
            callingWindow.UpdateTable(
                FirstRequestInput == "" ? null : FirstRequestInput,
                SecondRequestInput == "" ? null : SecondRequestInput
            );

            Close();
        }
    }
}
