using Simple_CRUD.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace Simple_CRUD.Presenter
{
    class Table
    {
        public String name;
        public List<String> types;
        public List<String> headers;
        public List<List<Object>> data;

        DataProvider provider;

        public Table(string name)
        {
            this.name = name;
            provider = new DataProvider();
            Refresh();
        }




        /// <summary>
        /// Актуализация данных о таблице в памяти приложения
        /// </summary>
        private void Refresh()
        {
            var information = provider.Select(name);
            types = information.Item1;
            headers = information.Item2;
            data = information.Item3;
        }

        
    }
}
