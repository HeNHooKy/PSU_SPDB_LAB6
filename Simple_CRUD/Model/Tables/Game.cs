using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Simple_CRUD.Model.Tables
{
    public class Game
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("budget")]
        public int Budget { get; set; }
        [Column("studio_id")]
        public int StudioId { get; set; }
        [Column("engine_id")]
        public int EngineId { get; set; }
        [Column("publisher_id")]
        public int PublisherId { get; set; }
         
        public Publisher Publisher { get; set; }
        public Engine Engine { get; set; }
        public Studio Studio { get; set; }
    }
}
