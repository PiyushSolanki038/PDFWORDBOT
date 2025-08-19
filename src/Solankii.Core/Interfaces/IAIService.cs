using Solankii.Core.Entities;

namespace Solankii.Core.Interfaces;

public interface IAIService
{
    Task<List<DesignSuggestion>> GenerateDesignSuggestionsAsync(DesignProject project, DesignComponent? component = null);
    Task<DesignComponent> GenerateComponentAsync(string description, ComponentType type, ComponentStyle style);
    Task<string> OptimizeCodeAsync(string code, string language, string optimizationType);
    Task<ComponentStyle> SuggestColorSchemeAsync(string description, string brandGuidelines);
    Task<string> GenerateAccessibilityImprovementsAsync(DesignComponent component);
    Task<string> GenerateResponsiveCodeAsync(string code, string framework);
    Task<double> AnalyzeUsabilityScoreAsync(DesignComponent component);
    Task<double> AnalyzeAccessibilityScoreAsync(DesignComponent component);
    Task<double> AnalyzePerformanceScoreAsync(DesignComponent component);
    Task<string> GenerateDocumentationAsync(DesignComponent component);
    Task<List<string>> SuggestImprovementsAsync(DesignComponent component, SuggestionType type);
    Task<string> TranslateDesignToCodeAsync(string designDescription, string targetFramework);
} 