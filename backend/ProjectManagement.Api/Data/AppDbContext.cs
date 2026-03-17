using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Models;

namespace ProjectManagement.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<AppRole> Roles => Set<AppRole>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<PortalDesign> PortalDesigns => Set<PortalDesign>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(company =>
        {
            company.ToTable("Company", "Master");
            company.Property(item => item.Name)
                .HasMaxLength(150);
            company.Property(item => item.Code)
                .HasMaxLength(50);
            company.Property(item => item.ContactEmail)
                .HasMaxLength(150);
            company.Property(item => item.ContactPhone)
                .HasMaxLength(25);
            company.Property(item => item.Website)
                .HasMaxLength(150);
            company.Property(item => item.AddressLine1)
                .HasMaxLength(200);
            company.Property(item => item.AddressLine2)
                .HasMaxLength(200);
            company.Property(item => item.City)
                .HasMaxLength(100);
            company.Property(item => item.State)
                .HasMaxLength(100);
            company.Property(item => item.Country)
                .HasMaxLength(100);
            company.Property(item => item.PostalCode)
                .HasMaxLength(20);
            company.HasIndex(item => item.Code)
                .IsUnique();
        });

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
            .Property(user => user.CompanyCode)
            .HasMaxLength(50);

        modelBuilder.Entity<ApplicationUser>()
            .Property(user => user.Email)
            .HasMaxLength(150);

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<PortalDesign>()
            .Property(design => design.CompanyCode)
            .HasMaxLength(50);

        modelBuilder.Entity<PortalDesign>()
            .Property(design => design.CustomerCode)
            .HasMaxLength(50);

        modelBuilder.Entity<PortalDesign>()
            .HasIndex(design => new { design.CompanyCode, design.CustomerCode })
            .IsUnique();
    }
}
