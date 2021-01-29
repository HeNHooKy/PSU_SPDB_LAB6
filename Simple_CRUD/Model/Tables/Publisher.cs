using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Simple_CRUD.Model.Tables
{
    public class Publisher
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("string")]
        public string Name { get; set; }
    }
}
