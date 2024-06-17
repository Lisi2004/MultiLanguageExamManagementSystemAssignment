using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Models.Entities;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    // DbSets for Exam Management System
    public DbSet<LocalUser> Users { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<ExamQuestion> ExamQuestions { get; set; }
    public DbSet<TakenExam> TakenExams { get; set; }

    // DbSets for Localization Management System
    public DbSet<Country> Countries { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<LocalizationResource> LocalizationResources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Exam Management System relationships
        modelBuilder.Entity<ExamQuestion>()
            .HasKey(eq => new { eq.ExamId, eq.QuestionId });

        modelBuilder.Entity<ExamQuestion>()
            .HasOne(eq => eq.Exam)
            .WithMany(e => e.ExamQuestions)
            .HasForeignKey(eq => eq.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExamQuestion>()
            .HasOne(eq => eq.Question)
            .WithMany(q => q.ExamQuestions)
            .HasForeignKey(eq => eq.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TakenExam>()
            .HasOne(te => te.Exam)
            .WithMany(e => e.TakenExams)
            .HasForeignKey(te => te.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TakenExam>()
            .HasOne(te => te.User)
            .WithMany(u => u.TakenExams)
            .HasForeignKey(te => te.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Change to Restrict to prevent multiple cascade paths

        // Localization Management System relationships
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
