using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Simple_CRUD.Model.Tables
{
    public class Engine
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("production_year")]
        public DateTime ProductionYear { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("studio_id")]
        public int StudioId { get; set; }
        public Studio Studio { get; set; }
    }
    public class Studio
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
    }
}
