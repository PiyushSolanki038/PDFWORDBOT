using Microsoft.EntityFrameworkCore;
using Solankii.Core.Entities;
using Solankii.Core.Interfaces;
using Solankii.Infrastructure.Data;

namespace Solankii.Infrastructure.Services;

public class ComponentService : IComponentService
{
    private readonly SolankiiDbContext _context;

    public ComponentService(SolankiiDbContext context)
    {
        _context = context;
    }

    public async Task<DesignComponent> CreateComponentAsync(DesignComponent component)
    {
        _context.Components.Add(component);
        await _context.SaveChangesAsync();
        return component;
    }

    public async Task<DesignComponent?> GetComponentByIdAsync(Guid id)
    {
        return await _context.Components
            .Include(c => c.Project)
            .Include(c => c.Variants)
            .Include(c => c.Properties)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<DesignComponent>> GetComponentsByProjectAsync(Guid projectId)
    {
        return await _context.Components
            .Include(c => c.Variants)
            .Include(c => c.Properties)
            .Where(c => c.ProjectId == projectId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<DesignComponent>> GetComponentsByTypeAsync(ComponentType type)
    {
        return await _context.Components
            .Include(c => c.Project)
            .Include(c => c.Variants)
            .Where(c => c.Type == type)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<DesignComponent> UpdateComponentAsync(DesignComponent component)
    {
        _context.Components.Update(component);
        await _context.SaveChangesAsync();
        return component;
    }

    public async Task<bool> DeleteComponentAsync(Guid id)
    {
        var component = await _context.Components.FindAsync(id);
        if (component == null)
        {
            return false;
        }

        _context.Components.Remove(component);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DesignComponent> DuplicateComponentAsync(Guid id, string newName)
    {
        var originalComponent = await GetComponentByIdAsync(id);
        if (originalComponent == null)
        {
            throw new ArgumentException("Component not found");
        }

        var duplicatedComponent = new DesignComponent
        {
            Id = Guid.NewGuid(),
            Name = newName,
            Description = originalComponent.Description,
            Type = originalComponent.Type,
            HtmlCode = originalComponent.HtmlCode,
            CssCode = originalComponent.CssCode,
            JsCode = originalComponent.JsCode,
            Style = originalComponent.Style,
            Accessibility = originalComponent.Accessibility,
            CreatedAt = DateTime.UtcNow,
            ProjectId = originalComponent.ProjectId
        };

        // Duplicate variants
        foreach (var variant in originalComponent.Variants)
        {
            var duplicatedVariant = new ComponentVariant
            {
                Id = Guid.NewGuid(),
                Name = variant.Name,
                Description = variant.Description,
                Style = variant.Style,
                HtmlCode = variant.HtmlCode,
                CssCode = variant.CssCode
            };

            duplicatedComponent.Variants.Add(duplicatedVariant);
        }

        // Duplicate properties
        foreach (var property in originalComponent.Properties)
        {
            var duplicatedProperty = new ComponentProperty
            {
                Id = Guid.NewGuid(),
                Name = property.Name,
                Type = property.Type,
                DefaultValue = property.DefaultValue,
                IsRequired = property.IsRequired,
                Description = property.Description
            };

            duplicatedComponent.Properties.Add(duplicatedProperty);
        }

        await CreateComponentAsync(duplicatedComponent);
        return duplicatedComponent;
    }

    public async Task<List<ComponentVariant>> GenerateVariantsAsync(DesignComponent component)
    {
        var variants = new List<ComponentVariant>();

        // Generate different style variants
        var variantStyles = new[]
        {
            new ComponentStyle { PrimaryColor = "#EF4444", BorderRadius = "4px" }, // Red variant
            new ComponentStyle { PrimaryColor = "#10B981", BorderRadius = "12px" }, // Green variant
            new ComponentStyle { PrimaryColor = "#8B5CF6", BorderRadius = "0px" }, // Purple variant
            new ComponentStyle { PrimaryColor = "#F59E0B", BorderRadius = "20px" }  // Orange variant
        };

        var variantNames = new[] { "Danger", "Success", "Purple", "Warning" };

        for (int i = 0; i < variantStyles.Length; i++)
        {
            var variant = new ComponentVariant
            {
                Id = Guid.NewGuid(),
                Name = $"{component.Name} - {variantNames[i]}",
                Description = $"{variantNames[i]} variant of {component.Name}",
                Style = variantStyles[i],
                HtmlCode = component.HtmlCode,
                CssCode = GenerateVariantCss(component.CssCode, variantStyles[i])
            };

            variants.Add(variant);
        }

        return variants;
    }

    public async Task<string> ExportComponentAsync(DesignComponent component, string format)
    {
        return format.ToLower() switch
        {
            "react" => ExportToReact(component),
            "vue" => ExportToVue(component),
            "angular" => ExportToAngular(component),
            "html" => ExportToHtml(component),
            _ => ExportToHtml(component)
        };
    }

    public async Task<DesignComponent> ImportComponentAsync(string code, string format, Guid projectId)
    {
        var component = new DesignComponent
        {
            Id = Guid.NewGuid(),
            Name = $"Imported Component - {DateTime.UtcNow:yyyyMMdd-HHmmss}",
            Description = $"Component imported from {format} format",
            Type = ComponentType.Custom,
            HtmlCode = code,
            CssCode = "",
            CreatedAt = DateTime.UtcNow,
            ProjectId = projectId
        };

        await CreateComponentAsync(component);
        return component;
    }

    public async Task<List<DesignComponent>> SearchComponentsAsync(string searchTerm)
    {
        return await _context.Components
            .Include(c => c.Project)
            .Where(c => c.Name.Contains(searchTerm) || 
                       c.Description.Contains(searchTerm) ||
                       c.HtmlCode.Contains(searchTerm))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<DesignComponent> OptimizeComponentAsync(DesignComponent component)
    {
        // Basic optimization logic
        component.HtmlCode = OptimizeHtml(component.HtmlCode);
        component.CssCode = OptimizeCss(component.CssCode);
        component.UpdatedAt = DateTime.UtcNow;

        await UpdateComponentAsync(component);
        return component;
    }

    private string GenerateVariantCss(string originalCss, ComponentStyle style)
    {
        // Replace color values in CSS with new variant colors
        var optimizedCss = originalCss
            .Replace("#3B82F6", style.PrimaryColor)
            .Replace("8px", style.BorderRadius);

        return optimizedCss;
    }

    private string ExportToReact(DesignComponent component)
    {
        var componentName = component.Name.Replace(" ", "").Replace("-", "");
        
        return $@"import React from 'react';
import './{componentName}.css';

interface {componentName}Props {{
  // Add your props here
}}

export const {componentName}: React.FC<{componentName}Props> = ({{ }}) => {{
  return (
    {component.HtmlCode}
  );
}};";
    }

    private string ExportToVue(DesignComponent component)
    {
        var componentName = component.Name.Replace(" ", "").Replace("-", "");
        
        return $@"<template>
  {component.HtmlCode}
</template>

<script lang=""ts"">
export default {{
  name: '{componentName}',
  props: {{
    // Add your props here
  }}
}}
</script>

<style scoped>
{component.CssCode}
</style>";
    }

    private string ExportToAngular(DesignComponent component)
    {
        var componentName = component.Name.Replace(" ", "").Replace("-", "");
        
        return $@"import {{ Component }} from '@angular/core';

@Component({{
  selector: 'app-{componentName.ToLower()}',
  template: `
    {component.HtmlCode}
  `,
  styles: [`
    {component.CssCode}
  `]
}})
export class {componentName}Component {{
  // Add your component logic here
}}";
    }

    private string ExportToHtml(DesignComponent component)
    {
        return $@"<!DOCTYPE html>
<html>
<head>
  <style>
    {component.CssCode}
  </style>
</head>
<body>
  {component.HtmlCode}
</body>
</html>";
    }

    private string OptimizeHtml(string html)
    {
        // Basic HTML optimization
        return html
            .Replace("  ", " ") // Remove extra spaces
            .Replace("\n\n", "\n") // Remove extra newlines
            .Trim();
    }

    private string OptimizeCss(string css)
    {
        // Basic CSS optimization
        return css
            .Replace("  ", " ") // Remove extra spaces
            .Replace(";}", "}") // Remove unnecessary semicolons
            .Replace("\n\n", "\n") // Remove extra newlines
            .Trim();
    }
} 