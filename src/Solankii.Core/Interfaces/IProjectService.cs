using Solankii.Core.Entities;

namespace Solankii.Core.Interfaces;

public interface IProjectService
{
    Task<DesignProject> CreateProjectAsync(DesignProject project);
    Task<DesignProject?> GetProjectByIdAsync(Guid id);
    Task<List<DesignProject>> GetAllProjectsAsync();
    Task<DesignProject> UpdateProjectAsync(DesignProject project);
    Task<bool> DeleteProjectAsync(Guid id);
    Task<List<DesignProject>> GetProjectsByTypeAsync(ProjectType type);
    Task<List<DesignProject>> GetProjectsByStatusAsync(ProjectStatus status);
    Task<DesignProject> DuplicateProjectAsync(Guid id, string newName);
    Task<ProjectSettings> UpdateProjectSettingsAsync(Guid projectId, ProjectSettings settings);
} 