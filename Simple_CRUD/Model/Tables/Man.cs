using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Simple_CRUD.Model.Tables
{
    public class Man
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("surname")]
        public string Surname { get; set; }
        [Column("country_born_id")]
        public int? CountryBornId { get; set; }

        public Country CountryBorn { get; set; }
    }
}
