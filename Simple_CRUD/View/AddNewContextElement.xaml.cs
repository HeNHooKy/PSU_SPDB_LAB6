using Simple_CRUD.Model;
using Simple_CRUD.Model.Tables;
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
    /// Логика взаимодействия для AddNewContextElement.xaml
    /// </summary>
    public partial class AddNewContextElement : Window
    {
        public string ElementName { get; set; }
        public RelayCommand AddElementCommand { get; private set; }

        private readonly IExtendedWindow callingWindow;
        private readonly Context context;
        private readonly int tableNum;
        private readonly object row;
        public AddNewContextElement(IExtendedWindow window, Context context, object row, int tableNum)
        {
            AddElementCommand = new RelayCommand(AddElementHandler);
            callingWindow = window;
            InitializeComponent();
            DataContext = this;
            this.context = context;
            this.tableNum = tableNum;
            this.row = row;
        }

        private void AddElementHandler()
        {
            if(ElementName != null && ElementName.Trim() != "")
            {
                AddToTable();
                callingWindow.UpdateTable();
                Close();
            }
        }

        private void AddToTable()
        {
            switch (tableNum)
            {
                case (int)Tables.Numbers.Country:
                    var country = (Country)row;
                    country.Name = ElementName;
                    context.Countries.Add(country);
                    break;
                case (int)Tables.Numbers.Engine:
                    var engine = (Engine)row;
                    engine.Name = ElementName;
                    context.Engines.Add(engine);
                    break;
                case (int)Tables.Numbers.Game:
                    var game = (Game)row;
                    game.Name = ElementName;
                    context.Games.Add(game);
                    break;
                case (int)Tables.Numbers.Man:
                    var man = (Man)row;
                    man.Name = ElementName;
                    context.People.Add(man);
                    break;
                case (int)Tables.Numbers.Playground:
                    var playground = (Playground)row;
                    playground.Name = ElementName;
                    context.Playgrounds.Add(playground);
                    break;
                case (int)Tables.Numbers.Publisher:
                    var publisher = (Publisher)row;
                    publisher.Name = ElementName;
                    context.Publishers.Add(publisher);
                    break;
                case (int)Tables.Numbers.Studio:
                    var studio = (Studio)row;
                    studio.Name = ElementName;
                    context.Studios.Add(studio);
                    break;
            }

            context.SaveChanges();
        }
    }
}
