using Npgsql.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple_CRUD.Model
{
    public class User
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public bool Approved { get; set; }
    }
}
