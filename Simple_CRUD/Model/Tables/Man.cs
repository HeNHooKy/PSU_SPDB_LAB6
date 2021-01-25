using System;
using System.Collections.Generic;
using System.Text;

namespace Simple_CRUD.Model.Tables
{
    public class Man : Row
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Country CountryBorn { get; set; }        
    }
}
