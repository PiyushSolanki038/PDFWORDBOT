using Microsoft.EntityFrameworkCore;
using Solankii.Core.Entities;

namespace Solankii.Infrastructure.Data;

public class SolankiiDbContext : DbContext
{
    public SolankiiDbContext(DbContextOptions<SolankiiDbContext> options) : base(options)
    {
    }

    public DbSet<DesignProject> Projects { get; set; }
    public DbSet<DesignComponent> Components { get; set; }
    public DbSet<DesignSuggestion> Suggestions { get; set; }
    public DbSet<DesignReview> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure DesignProject
        modelBuilder.Entity<DesignProject>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.Status).HasDefaultValue(ProjectStatus.Draft);
            
            // Configure complex types
            entity.OwnsOne(e => e.Settings, settings =>
            {
                settings.ToJson();
            });
        });

        // Configure DesignComponent
        modelBuilder.Entity<DesignComponent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.HtmlCode).IsRequired();
            entity.Property(e => e.CssCode).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            // Configure complex types
            entity.OwnsOne(e => e.Style, style =>
            {
                style.ToJson();
            });
            
            entity.OwnsOne(e => e.Accessibility, accessibility =>
            {
                accessibility.ToJson();
            });
            
            entity.HasMany(e => e.Variants)
                  .WithOne()
                  .HasForeignKey("ComponentId")
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasMany(e => e.Properties)
                  .WithOne()
                  .HasForeignKey("ComponentId")
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.Components)
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure ComponentVariant
        modelBuilder.Entity<ComponentVariant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.OwnsOne(e => e.Style, style =>
            {
                style.ToJson();
            });
        });

        // Configure ComponentProperty
        modelBuilder.Entity<ComponentProperty>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DefaultValue).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // Configure DesignSuggestion
        modelBuilder.Entity<DesignSuggestion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.GeneratedCode).IsRequired();
            entity.Property(e => e.CodeLanguage).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.Priority).HasDefaultValue(SuggestionPriority.Medium);
            
            entity.OwnsOne(e => e.Metrics, metrics =>
            {
                metrics.ToJson();
            });
            
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.Suggestions)
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Component)
                  .WithMany()
                  .HasForeignKey(e => e.ComponentId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure DesignReview
        modelBuilder.Entity<DesignReview>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.ReviewerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ReviewerEmail).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.Status).HasDefaultValue(ReviewStatus.Pending);
            
            entity.OwnsOne(e => e.Criteria, criteria =>
            {
                criteria.ToJson();
            });
            
            entity.OwnsOne(e => e.Score, score =>
            {
                score.ToJson();
            });
            
            entity.HasMany(e => e.Comments)
                  .WithOne()
                  .HasForeignKey("ReviewId")
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Project)
                  .WithMany(p => p.Reviews)
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Component)
                  .WithMany()
                  .HasForeignKey(e => e.ComponentId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure ReviewComment
        modelBuilder.Entity<ReviewComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Author).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.Resolution).HasMaxLength(1000);
        });
    }
} 