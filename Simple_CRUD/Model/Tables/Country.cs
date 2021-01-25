using System;
using System.Collections.Generic;
using System.Text;

namespace Simple_CRUD.Model.Tables
{
    public class Country : Row
    {
        public string Land { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
    }
}
