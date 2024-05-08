using FamilyTree.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FamilyTree.Context
{
    public class HalflingContext : DbContext
    {
        public HalflingContext()
        {
        }

        public HalflingContext(DbContextOptions<HalflingContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<Halfling> Halflings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=HalfingDB",
                    sqlServerOptionsBuilder => sqlServerOptionsBuilder.UseHierarchyId());

        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            ModelBuilder.Entity<Halfling>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();
        }
    }
}
