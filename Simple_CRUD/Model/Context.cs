using Microsoft.EntityFrameworkCore;
using Simple_CRUD.Model.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple_CRUD.Model
{
    public class Context : DbContext
    {
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");

    }
}
