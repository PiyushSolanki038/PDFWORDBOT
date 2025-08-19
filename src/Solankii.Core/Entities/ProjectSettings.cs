namespace Solankii.Core.Entities;

public class ProjectSettings
{
    public Guid Id { get; set; }
    
    public DesignSystem DesignSystem { get; set; } = new();
    
    public AISettings AI { get; set; } = new();
    
    public CollaborationSettings Collaboration { get; set; } = new();
    
    public ExportSettings Export { get; set; } = new();
    
    public NotificationSettings Notifications { get; set; } = new();
}

public class DesignSystem
{
    public string Name { get; set; } = "Default Design System";
    public string Version { get; set; } = "1.0.0";
    public ColorPalette Colors { get; set; } = new();
    public TypographySettings Typography { get; set; } = new();
    public SpacingSettings Spacing { get; set; } = new();
    public List<string> SupportedFrameworks { get; set; } = new() { "React", "Vue", "Angular", "HTML/CSS" };
    public bool EnableDarkMode { get; set; } = true;
    public bool EnableResponsiveDesign { get; set; } = true;
}

public class ColorPalette
{
    public string Primary { get; set; } = "#3B82F6";
    public string Secondary { get; set; } = "#6B7280";
    public string Success { get; set; } = "#10B981";
    public string Warning { get; set; } = "#F59E0B";
    public string Error { get; set; } = "#EF4444";
    public string Info { get; set; } = "#06B6D4";
    public string Background { get; set; } = "#FFFFFF";
    public string Surface { get; set; } = "#F9FAFB";
    public string Text { get; set; } = "#1F2937";
    public string TextSecondary { get; set; } = "#6B7280";
}

public class TypographySettings
{
    public string FontFamily { get; set; } = "Inter, sans-serif";
    public string FontSizeBase { get; set; } = "16px";
    public string LineHeight { get; set; } = "1.5";
    public string FontWeightNormal { get; set; } = "400";
    public string FontWeightMedium { get; set; } = "500";
    public string FontWeightBold { get; set; } = "700";
}

public class SpacingSettings
{
    public string Xs { get; set; } = "4px";
    public string Sm { get; set; } = "8px";
    public string Md { get; set; } = "16px";
    public string Lg { get; set; } = "24px";
    public string Xl { get; set; } = "32px";
    public string Xxl { get; set; } = "48px";
}

public class AISettings
{
    public bool EnableAISuggestions { get; set; } = true;
    public bool EnableAutoCodeGeneration { get; set; } = true;
    public bool EnableAccessibilityAnalysis { get; set; } = true;
    public bool EnablePerformanceOptimization { get; set; } = true;
    public string PreferredAIModel { get; set; } = "GPT-4";
    public List<string> EnabledFeatures { get; set; } = new() 
    { 
        "ComponentGeneration", 
        "LayoutOptimization", 
        "ColorSchemeSuggestions", 
        "AccessibilityImprovements" 
    };
    public int MaxSuggestionsPerComponent { get; set; } = 5;
}

public class CollaborationSettings
{
    public bool EnableRealTimeCollaboration { get; set; } = true;
    public bool EnableComments { get; set; } = true;
    public bool EnableVersionControl { get; set; } = true;
    public bool EnableApprovalWorkflow { get; set; } = true;
    public List<string> AllowedRoles { get; set; } = new() { "Designer", "Developer", "Reviewer", "Stakeholder" };
    public bool RequireApprovalForChanges { get; set; } = false;
}

public class ExportSettings
{
    public List<string> ExportFormats { get; set; } = new() { "HTML", "CSS", "React", "Vue", "Angular", "Figma" };
    public bool IncludeDocumentation { get; set; } = true;
    public bool IncludeAccessibilityInfo { get; set; } = true;
    public bool IncludeResponsiveCode { get; set; } = true;
    public string DefaultExportFormat { get; set; } = "React";
}

public class NotificationSettings
{
    public bool EmailNotifications { get; set; } = true;
    public bool InAppNotifications { get; set; } = true;
    public bool SlackNotifications { get; set; } = false;
    public List<string> NotificationEvents { get; set; } = new() 
    { 
        "ProjectCreated", 
        "ComponentUpdated", 
        "ReviewRequested", 
        "SuggestionGenerated" 
    };
} 