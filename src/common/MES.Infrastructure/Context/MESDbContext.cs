using MES.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MES.Infrastructure.Context;

public partial class MESDbContext : DbContext
{
    public DbSet<UserCredential> Users => Set<UserCredential>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    public MESDbContext(DbContextOptions<MESDbContext> options)
        : base(options) { }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=MES;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 如果有 Fluent API 配置，可以这里做
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MESDbContext).Assembly);
        
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
