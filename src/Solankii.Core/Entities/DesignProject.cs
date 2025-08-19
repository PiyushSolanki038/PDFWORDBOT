using System.ComponentModel.DataAnnotations;

namespace Solankii.Core.Entities;

public class DesignProject
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public ProjectType Type { get; set; }
    
    public ProjectStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    
    public List<DesignComponent> Components { get; set; } = new();
    
    public List<DesignSuggestion> Suggestions { get; set; } = new();
    
    public List<DesignReview> Reviews { get; set; } = new();
    
    public ProjectSettings Settings { get; set; } = new();
}

public enum ProjectType
{
    WebApplication,
    MobileApp,
    Dashboard,
    LandingPage,
    ECommerce,
    Portfolio,
    Blog,
    AdminPanel,
    Custom
}

public enum ProjectStatus
{
    Draft,
    InProgress,
    Review,
    Approved,
    Completed,
    Archived
} 