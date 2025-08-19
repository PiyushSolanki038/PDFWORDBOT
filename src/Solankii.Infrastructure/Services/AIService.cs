using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Solankii.Core.Entities;
using Solankii.Core.Interfaces;
using System.Text;

namespace Solankii.Infrastructure.Services;

public class AIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AIService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private readonly string _apiEndpoint;

    public AIService(HttpClient httpClient, ILogger<AIService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
        _apiKey = _configuration["OpenAI:ApiKey"] ?? "";
        _apiEndpoint = _configuration["OpenAI:Endpoint"] ?? "https://api.openai.com/v1/chat/completions";
    }

    public async Task<List<DesignSuggestion>> GenerateDesignSuggestionsAsync(DesignProject project, DesignComponent? component = null)
    {
        try
        {
            var suggestions = new List<DesignSuggestion>();
            var prompt = GenerateSuggestionPrompt(project, component);
            
            var response = await CallOpenAIAsync(prompt);
            var suggestionsData = ParseSuggestionsResponse(response);
            
            foreach (var suggestionData in suggestionsData)
            {
                var suggestion = new DesignSuggestion
                {
                    Id = Guid.NewGuid(),
                    Title = suggestionData.Title,
                    Description = suggestionData.Description,
                    Type = suggestionData.Type,
                    Priority = suggestionData.Priority,
                    GeneratedCode = suggestionData.Code,
                    CodeLanguage = suggestionData.Language,
                    Tags = suggestionData.Tags,
                    Metrics = suggestionData.Metrics,
                    ProjectId = project.Id,
                    ComponentId = component?.Id,
                    CreatedAt = DateTime.UtcNow
                };
                
                suggestions.Add(suggestion);
            }
            
            return suggestions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating design suggestions");
            return new List<DesignSuggestion>();
        }
    }

    public async Task<DesignComponent> GenerateComponentAsync(string description, ComponentType type, ComponentStyle style)
    {
        try
        {
            var prompt = GenerateComponentPrompt(description, type, style);
            var response = await CallOpenAIAsync(prompt);
            var componentData = ParseComponentResponse(response);
            
            return new DesignComponent
            {
                Id = Guid.NewGuid(),
                Name = componentData.Name,
                Description = description,
                Type = type,
                HtmlCode = componentData.HtmlCode,
                CssCode = componentData.CssCode,
                JsCode = componentData.JsCode,
                Style = style,
                Properties = componentData.Properties,
                Accessibility = componentData.Accessibility,
                CreatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating component");
            throw;
        }
    }

    public async Task<string> OptimizeCodeAsync(string code, string language, string optimizationType)
    {
        try
        {
            var prompt = GenerateOptimizationPrompt(code, language, optimizationType);
            var response = await CallOpenAIAsync(prompt);
            return ParseCodeResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error optimizing code");
            return code;
        }
    }

    public async Task<ComponentStyle> SuggestColorSchemeAsync(string description, string brandGuidelines)
    {
        try
        {
            var prompt = GenerateColorSchemePrompt(description, brandGuidelines);
            var response = await CallOpenAIAsync(prompt);
            return ParseColorSchemeResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error suggesting color scheme");
            return new ComponentStyle();
        }
    }

    public async Task<string> GenerateAccessibilityImprovementsAsync(DesignComponent component)
    {
        try
        {
            var prompt = GenerateAccessibilityPrompt(component);
            var response = await CallOpenAIAsync(prompt);
            return ParseCodeResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating accessibility improvements");
            return component.HtmlCode;
        }
    }

    public async Task<string> GenerateResponsiveCodeAsync(string code, string framework)
    {
        try
        {
            var prompt = GenerateResponsivePrompt(code, framework);
            var response = await CallOpenAIAsync(prompt);
            return ParseCodeResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating responsive code");
            return code;
        }
    }

    public async Task<double> AnalyzeUsabilityScoreAsync(DesignComponent component)
    {
        try
        {
            var prompt = GenerateUsabilityAnalysisPrompt(component);
            var response = await CallOpenAIAsync(prompt);
            return ParseScoreResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing usability score");
            return 7.0; // Default score
        }
    }

    public async Task<double> AnalyzeAccessibilityScoreAsync(DesignComponent component)
    {
        try
        {
            var prompt = GenerateAccessibilityAnalysisPrompt(component);
            var response = await CallOpenAIAsync(prompt);
            return ParseScoreResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing accessibility score");
            return 7.0; // Default score
        }
    }

    public async Task<double> AnalyzePerformanceScoreAsync(DesignComponent component)
    {
        try
        {
            var prompt = GeneratePerformanceAnalysisPrompt(component);
            var response = await CallOpenAIAsync(prompt);
            return ParseScoreResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing performance score");
            return 7.0; // Default score
        }
    }

    public async Task<string> GenerateDocumentationAsync(DesignComponent component)
    {
        try
        {
            var prompt = GenerateDocumentationPrompt(component);
            var response = await CallOpenAIAsync(prompt);
            return ParseCodeResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating documentation");
            return "";
        }
    }

    public async Task<List<string>> SuggestImprovementsAsync(DesignComponent component, SuggestionType type)
    {
        try
        {
            var prompt = GenerateImprovementsPrompt(component, type);
            var response = await CallOpenAIAsync(prompt);
            return ParseImprovementsResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error suggesting improvements");
            return new List<string>();
        }
    }

    public async Task<string> TranslateDesignToCodeAsync(string designDescription, string targetFramework)
    {
        try
        {
            var prompt = GenerateTranslationPrompt(designDescription, targetFramework);
            var response = await CallOpenAIAsync(prompt);
            return ParseCodeResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error translating design to code");
            return "";
        }
    }

    private async Task<string> CallOpenAIAsync(string prompt)
    {
        var requestBody = new
        {
            model = "gpt-4",
            messages = new[]
            {
                new { role = "system", content = "You are an expert UI/UX designer and frontend developer. Provide practical, modern, and accessible design solutions." },
                new { role = "user", content = prompt }
            },
            max_tokens = 2000,
            temperature = 0.7
        };

        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        
        var response = await _httpClient.PostAsync(_apiEndpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"OpenAI API error: {responseContent}");
        }
        
        var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
        return result?.choices?[0]?.message?.content?.ToString() ?? "";
    }

    private string GenerateSuggestionPrompt(DesignProject project, DesignComponent? component)
    {
        var basePrompt = $"Generate design suggestions for a {project.Type} project named '{project.Name}'.";
        
        if (component != null)
        {
            basePrompt += $"\n\nFocus on the {component.Type} component: {component.Name}";
            basePrompt += $"\nCurrent HTML: {component.HtmlCode}";
            basePrompt += $"\nCurrent CSS: {component.CssCode}";
        }
        
        basePrompt += "\n\nProvide 3-5 specific, actionable suggestions with:";
        basePrompt += "\n- Title";
        basePrompt += "\n- Description";
        basePrompt += "\n- Type (LayoutImprovement, ColorScheme, Typography, Accessibility, Performance, etc.)";
        basePrompt += "\n- Priority (Low, Medium, High, Critical)";
        basePrompt += "\n- Generated code";
        basePrompt += "\n- Metrics (usability, accessibility, performance scores)";
        basePrompt += "\n- Tags";
        
        return basePrompt;
    }

    private string GenerateComponentPrompt(string description, ComponentType type, ComponentStyle style)
    {
        return $@"Generate a {type} component based on this description: {description}

Style requirements:
- Primary color: {style.PrimaryColor}
- Secondary color: {style.SecondaryColor}
- Background color: {style.BackgroundColor}
- Text color: {style.TextColor}
- Border radius: {style.BorderRadius}
- Font family: {style.FontFamily}
- Font size: {style.FontSize}
- Spacing: {style.Spacing}

Provide:
1. Component name
2. HTML code
3. CSS code
4. JavaScript code (if needed)
5. Component properties
6. Accessibility features

Make it modern, accessible, and responsive.";
    }

    private string GenerateOptimizationPrompt(string code, string language, string optimizationType)
    {
        return $@"Optimize this {language} code for {optimizationType}:

{code}

Provide the optimized code with explanations of the improvements made.";
    }

    private string GenerateColorSchemePrompt(string description, string brandGuidelines)
    {
        return $@"Suggest a color scheme for: {description}

Brand guidelines: {brandGuidelines}

Provide a JSON object with:
- Primary color
- Secondary color
- Background color
- Text color
- Success, warning, error colors
- Surface colors

Ensure good contrast ratios and accessibility compliance.";
    }

    private string GenerateAccessibilityPrompt(DesignComponent component)
    {
        return $@"Improve the accessibility of this component:

HTML: {component.HtmlCode}
CSS: {component.CssCode}

Provide improved HTML with:
- Proper ARIA labels
- Semantic HTML
- Keyboard navigation
- Screen reader support
- Focus indicators
- Color contrast improvements";
    }

    private string GenerateResponsivePrompt(string code, string framework)
    {
        return $@"Make this code responsive for {framework}:

{code}

Provide responsive code that works on:
- Mobile (320px+)
- Tablet (768px+)
- Desktop (1024px+)

Use modern responsive techniques like CSS Grid, Flexbox, and media queries.";
    }

    private string GenerateUsabilityAnalysisPrompt(DesignComponent component)
    {
        return $@"Analyze the usability of this component:

HTML: {component.HtmlCode}
CSS: {component.CssCode}

Rate from 1-10 and provide a score based on:
- Clarity and intuitiveness
- User interaction patterns
- Visual hierarchy
- Information architecture
- User feedback mechanisms";
    }

    private string GenerateAccessibilityAnalysisPrompt(DesignComponent component)
    {
        return $@"Analyze the accessibility of this component:

HTML: {component.HtmlCode}
CSS: {component.CssCode}

Rate from 1-10 and provide a score based on:
- WCAG 2.1 compliance
- Keyboard navigation
- Screen reader compatibility
- Color contrast
- Focus management
- Semantic HTML usage";
    }

    private string GeneratePerformanceAnalysisPrompt(DesignComponent component)
    {
        return $@"Analyze the performance of this component:

HTML: {component.HtmlCode}
CSS: {component.CssCode}
JS: {component.JsCode}

Rate from 1-10 and provide a score based on:
- CSS efficiency
- JavaScript optimization
- Asset loading
- Rendering performance
- Memory usage";
    }

    private string GenerateDocumentationPrompt(DesignComponent component)
    {
        return $@"Generate documentation for this component:

Name: {component.Name}
Type: {component.Type}
HTML: {component.HtmlCode}
CSS: {component.CssCode}

Provide comprehensive documentation including:
- Usage examples
- Props/parameters
- Accessibility features
- Browser support
- Best practices";
    }

    private string GenerateImprovementsPrompt(DesignComponent component, SuggestionType type)
    {
        return $@"Suggest {type} improvements for this component:

Name: {component.Name}
Type: {component.Type}
HTML: {component.HtmlCode}
CSS: {component.CssCode}

Provide specific, actionable improvement suggestions.";
    }

    private string GenerateTranslationPrompt(string designDescription, string targetFramework)
    {
        return $@"Translate this design description to {targetFramework} code:

{designDescription}

Provide clean, modern, and accessible code that follows {targetFramework} best practices.";
    }

    private List<SuggestionData> ParseSuggestionsResponse(string response)
    {
        // Parse AI response and extract suggestion data
        // This is a simplified implementation
        var suggestions = new List<SuggestionData>();
        
        // Parse the response and create SuggestionData objects
        // In a real implementation, you'd parse the structured response
        
        return suggestions;
    }

    private ComponentData ParseComponentResponse(string response)
    {
        // Parse AI response and extract component data
        return new ComponentData();
    }

    private string ParseCodeResponse(string response)
    {
        // Extract code from AI response
        return response;
    }

    private ComponentStyle ParseColorSchemeResponse(string response)
    {
        // Parse color scheme from AI response
        return new ComponentStyle();
    }

    private double ParseScoreResponse(string response)
    {
        // Parse score from AI response
        if (double.TryParse(response, out double score))
        {
            return Math.Max(1.0, Math.Min(10.0, score));
        }
        return 7.0;
    }

    private List<string> ParseImprovementsResponse(string response)
    {
        // Parse improvements from AI response
        return response.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
    }

    // Helper classes for parsing
    private class SuggestionData
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public SuggestionType Type { get; set; }
        public SuggestionPriority Priority { get; set; }
        public string Code { get; set; } = "";
        public string Language { get; set; } = "";
        public List<string> Tags { get; set; } = new();
        public SuggestionMetrics Metrics { get; set; } = new();
    }

    private class ComponentData
    {
        public string Name { get; set; } = "";
        public string HtmlCode { get; set; } = "";
        public string CssCode { get; set; } = "";
        public string JsCode { get; set; } = "";
        public List<ComponentProperty> Properties { get; set; } = new();
        public AccessibilityInfo Accessibility { get; set; } = new();
    }
} 