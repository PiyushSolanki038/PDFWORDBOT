using System.ComponentModel.DataAnnotations;

namespace Solankii.Core.Entities;

public class DesignSuggestion
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;
    
    public SuggestionType Type { get; set; }
    
    public SuggestionPriority Priority { get; set; }
    
    public string GeneratedCode { get; set; } = string.Empty;
    
    public string? CodeLanguage { get; set; }
    
    public List<string> Tags { get; set; } = new();
    
    public SuggestionMetrics Metrics { get; set; } = new();
    
    public bool IsApplied { get; set; } = false;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? AppliedAt { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public DesignProject Project { get; set; } = null!;
    
    public Guid? ComponentId { get; set; }
    
    public DesignComponent? Component { get; set; }
}

public enum SuggestionType
{
    LayoutImprovement,
    ColorScheme,
    Typography,
    Accessibility,
    Performance,
    ResponsiveDesign,
    Animation,
    Interaction,
    Content,
    Branding,
    UserExperience,
    CodeOptimization
}

public enum SuggestionPriority
{
    Low,
    Medium,
    High,
    Critical
}

public class SuggestionMetrics
{
    public double UsabilityScore { get; set; }
    public double AccessibilityScore { get; set; }
    public double PerformanceScore { get; set; }
    public double AestheticsScore { get; set; }
    public int EstimatedImplementationTime { get; set; } // in minutes
    public string ImpactLevel { get; set; } = "Medium";
    public List<string> Benefits { get; set; } = new();
    public List<string> Risks { get; set; } = new();
} 