using Microsoft.EntityFrameworkCore;
using Simple_CRUD.Model.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Simple_CRUD.Model
{
    public class Context : DbContext
    {
        public AuthUser User { get; set; }
        public Context(AuthUser User) : base()
        {
            this.User = User;
        }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Man> People { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Price> AllPrices { get; set; }
        public DbSet<Playground> Playgrounds { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(Config.connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Price>()
                .ToTable("allprices");

            modelBuilder.Entity<Country>()
                .ToTable("countries")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Man>()
                .ToTable("people")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Playground>()
                .ToTable("playgrounds")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Game>()
                .ToTable("games")
                .HasKey(g => g.Id);

            modelBuilder.Entity<Engine>()
                .ToTable("engines")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Publisher>()
                .ToTable("publishers")
                .HasKey(p => p.Id);

            modelBuilder.Entity<Studio>()
                .ToTable("studios")
                .HasKey(s => s.Id);

            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasKey(u => u.Id);
        }

    }
}
