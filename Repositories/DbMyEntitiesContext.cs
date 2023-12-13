using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Reflection;

namespace Bank
{
    internal class DbMyEntitiesContext : DbContext
    {
        public DbMyEntitiesContext(DbContextOptions<DbMyEntitiesContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
