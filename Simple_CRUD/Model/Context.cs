using Microsoft.EntityFrameworkCore;
using Simple_CRUD.Model.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple_CRUD.Model
{
    public class Context : DbContext
    {
        public User User { get; set; }
        public Context(User User) : base()
        {
            this.User = User;
        }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Engine> Engines { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host = localhost; Port = 5432; Database = postgres; Username = postgres; Password = pereterebi123");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Engine>()
                .ToTable("engines")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Game>()
                .ToTable("games")
                .HasKey(g => g.Id);

            modelBuilder.Entity<Studio>()
                .ToTable("studios")
                .HasKey(s => s.Id);

            modelBuilder.Entity<Publisher>()
                .ToTable("publishers")
                .HasKey(p => p.Id);
        }

                
    }
}
