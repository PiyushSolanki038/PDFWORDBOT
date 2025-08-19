using Solankii.Core.Entities;

namespace Solankii.Core.Interfaces;

public interface IComponentService
{
    Task<DesignComponent> CreateComponentAsync(DesignComponent component);
    Task<DesignComponent?> GetComponentByIdAsync(Guid id);
    Task<List<DesignComponent>> GetComponentsByProjectAsync(Guid projectId);
    Task<List<DesignComponent>> GetComponentsByTypeAsync(ComponentType type);
    Task<DesignComponent> UpdateComponentAsync(DesignComponent component);
    Task<bool> DeleteComponentAsync(Guid id);
    Task<DesignComponent> DuplicateComponentAsync(Guid id, string newName);
    Task<List<ComponentVariant>> GenerateVariantsAsync(DesignComponent component);
    Task<string> ExportComponentAsync(DesignComponent component, string format);
    Task<DesignComponent> ImportComponentAsync(string code, string format, Guid projectId);
    Task<List<DesignComponent>> SearchComponentsAsync(string searchTerm);
    Task<DesignComponent> OptimizeComponentAsync(DesignComponent component);
} 