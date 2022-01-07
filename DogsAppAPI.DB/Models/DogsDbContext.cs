using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
