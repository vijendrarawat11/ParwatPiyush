using Microsoft.EntityFrameworkCore;
using ParwatPiyushNewsPortal.Models;
namespace ParwatPiyushNewsPortal.Data
{
    public class ParwatPiyushDB : DbContext
    {
        public ParwatPiyushDB(DbContextOptions<ParwatPiyushDB> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Topics> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>()
                .HasOne(n => n.Author)
                .WithMany()
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // Ensure correct behavior

            base.OnModelCreating(modelBuilder);
        }
    }

}
