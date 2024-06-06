using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Models.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<LocalizationResource> LocalizationResources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>()
            .HasMany(c => c.Languages)
            .WithOne(l => l.Country)
            .HasForeignKey(l => l.CountryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Language>()
            .HasMany(l => l.LocalizationResources)
            .WithOne(lr => lr.Language)
            .HasForeignKey(lr => lr.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LocalizationResource>()
            .HasIndex(lr => new { lr.Namespace, lr.Key })
            .IsUnique();
    }
}
