using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DogsAppAPI.DB.Models
{
    public class DogsDbContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        public DogsDbContext(DbContextOptions<DogsDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Dog>(entity =>
            {
                entity.HasKey(k => k.ID).HasName("ID");
            });
        }
    }
}
