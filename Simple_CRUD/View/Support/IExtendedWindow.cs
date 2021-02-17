using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Simple_CRUD.View.Support
{
    public interface IExtendedWindow
    {
        void UpdateTable(string firstField, string secondField);
        void UpdateTable();
    }
}
