using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Simple_CRUD.Model.Tables
{
    public class Price
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("price")]
        public float Cost { get; set; }
        [Column("game_id")]
        public int GameId { get; set; }
        [Column("playground_id")]
        public int PlaygroundId { get; set; }
        public Game Game { get; set; }
        public Playground Playground { get; set; }
        
    }
}
