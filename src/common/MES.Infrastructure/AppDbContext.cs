using MES.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MES.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<UserCredential> Users => Set<UserCredential>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<UserProfile>()
            .HasOne(u => u.Credential)
            .WithOne(c => c.UserProfile)
            .HasForeignKey<UserCredential>(c => c.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
