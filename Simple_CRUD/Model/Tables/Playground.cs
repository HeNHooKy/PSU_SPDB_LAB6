using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Simple_CRUD.Model.Tables
{
    public class Playground
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("country_id")]
        public int CountryId { get; set; }
        [Column("employer_id")]
        public int EmployerId { get; set; }
        public Country Country { get; set; }
        public Man Employer { get; set; }

        internal static object Select(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
