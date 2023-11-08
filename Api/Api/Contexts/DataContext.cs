using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;

namespace Api.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(user =>
            {
                user.HasIndex(u => u.Username)
                .IsUnique();

                user.HasMany(a => a.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            });

          
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AddressEntity> Addresses { get; set; }
    }
}
