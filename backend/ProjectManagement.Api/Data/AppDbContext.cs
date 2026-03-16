using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppRole> Roles => Set<AppRole>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<PortalDesign> PortalDesigns => Set<PortalDesign>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppRole>(role =>
        {
            role.ToTable("Role", "Master");
            role.Property(item => item.Name)
                .HasMaxLength(100);
            role.Property(item => item.Description)
                .HasMaxLength(250);
            role.HasIndex(item => item.Name)
                .IsUnique();
        });

        modelBuilder.Entity<ApplicationUser>()
            .Property(user => user.Role)
            .HasMaxLength(100);

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<PortalDesign>()
            .HasIndex(design => design.CustomerCode)
            .IsUnique();
    }
}
