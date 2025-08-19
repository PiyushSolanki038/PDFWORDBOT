using System.ComponentModel.DataAnnotations;

namespace Solankii.Core.Entities;

public class DesignReview
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;
    
    public ReviewStatus Status { get; set; }
    
    public ReviewType Type { get; set; }
    
    public string ReviewerName { get; set; } = string.Empty;
    
    public string ReviewerEmail { get; set; } = string.Empty;
    
    public ReviewCriteria Criteria { get; set; } = new();
    
    public List<ReviewComment> Comments { get; set; } = new();
    
    public ReviewScore Score { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public DesignProject Project { get; set; } = null!;
    
    public Guid? ComponentId { get; set; }
    
    public DesignComponent? Component { get; set; }
}

public enum ReviewStatus
{
    Pending,
    InProgress,
    Completed,
    Approved,
    Rejected,
    NeedsRevision
}

public enum ReviewType
{
    DesignReview,
    AccessibilityReview,
    CodeReview,
    UXReview,
    PerformanceReview,
    SecurityReview
}

public class ReviewCriteria
{
    public bool Usability { get; set; } = true;
    public bool Accessibility { get; set; } = true;
    public bool Performance { get; set; } = true;
    public bool Aesthetics { get; set; } = true;
    public bool BrandConsistency { get; set; } = true;
    public bool CodeQuality { get; set; } = true;
    public bool ResponsiveDesign { get; set; } = true;
    public bool CrossBrowserCompatibility { get; set; } = true;
}

public class ReviewComment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public CommentType Type { get; set; }
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsResolved { get; set; } = false;
    public string? Resolution { get; set; }
    public List<string> Attachments { get; set; } = new();
}

public enum CommentType
{
    Suggestion,
    Issue,
    Question,
    Praise,
    General
}

public class ReviewScore
{
    public double OverallScore { get; set; }
    public double UsabilityScore { get; set; }
    public double AccessibilityScore { get; set; }
    public double PerformanceScore { get; set; }
    public double AestheticsScore { get; set; }
    public double CodeQualityScore { get; set; }
    public string Summary { get; set; } = string.Empty;
    public List<string> Strengths { get; set; } = new();
    public List<string> AreasForImprovement { get; set; } = new();
} 