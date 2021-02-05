using Npgsql.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_CRUD.Model
{
    public class User
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("id")]
        public int Id { get; set; }
        [Column("login")]
        public string Login { get; set; }
        [Column("password_hash")]
        public string PasswordHash { get; set; }
    }

}
