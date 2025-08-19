using System.ComponentModel.DataAnnotations;

namespace Solankii.Core.Entities;

public class DesignComponent
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public ComponentType Type { get; set; }
    
    public string HtmlCode { get; set; } = string.Empty;
    
    public string CssCode { get; set; } = string.Empty;
    
    public string? JsCode { get; set; }
    
    public ComponentStyle Style { get; set; } = new();
    
    public List<ComponentVariant> Variants { get; set; } = new();
    
    public List<ComponentProperty> Properties { get; set; } = new();
    
    public AccessibilityInfo Accessibility { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public Guid ProjectId { get; set; }
    
    public DesignProject Project { get; set; } = null!;
}

public enum ComponentType
{
    Button,
    Input,
    Card,
    Modal,
    Navigation,
    Form,
    Table,
    Chart,
    Avatar,
    Badge,
    Alert,
    Progress,
    Tabs,
    Accordion,
    Dropdown,
    Tooltip,
    Custom
}

public class ComponentStyle
{
    public string PrimaryColor { get; set; } = "#3B82F6";
    public string SecondaryColor { get; set; } = "#6B7280";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string TextColor { get; set; } = "#1F2937";
    public string BorderRadius { get; set; } = "8px";
    public string FontFamily { get; set; } = "Inter, sans-serif";
    public string FontSize { get; set; } = "14px";
    public string Spacing { get; set; } = "16px";
    public bool IsDarkMode { get; set; } = false;
}

public class ComponentVariant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ComponentStyle Style { get; set; } = new();
    public string HtmlCode { get; set; } = string.Empty;
    public string CssCode { get; set; } = string.Empty;
}

public class ComponentProperty
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string DefaultValue { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = false;
    public string? Description { get; set; }
}

public class AccessibilityInfo
{
    public string AriaLabel { get; set; } = string.Empty;
    public string AriaDescribedBy { get; set; } = string.Empty;
    public bool IsKeyboardAccessible { get; set; } = true;
    public bool HasFocusIndicator { get; set; } = true;
    public string ColorContrast { get; set; } = "4.5:1";
    public List<string> ScreenReaderText { get; set; } = new();
} 