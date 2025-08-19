using Microsoft.EntityFrameworkCore;
using Solankii.Core.Entities;
using Solankii.Core.Interfaces;
using Solankii.Infrastructure.Data;

namespace Solankii.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly SolankiiDbContext _context;

    public ProjectService(SolankiiDbContext context)
    {
        _context = context;
    }

    public async Task<DesignProject> CreateProjectAsync(DesignProject project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<DesignProject?> GetProjectByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.Components)
            .Include(p => p.Suggestions)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<DesignProject>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Components)
            .Include(p => p.Suggestions)
            .Include(p => p.Reviews)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<DesignProject> UpdateProjectAsync(DesignProject project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return false;
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<DesignProject>> GetProjectsByTypeAsync(ProjectType type)
    {
        return await _context.Projects
            .Include(p => p.Components)
            .Where(p => p.Type == type)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<DesignProject>> GetProjectsByStatusAsync(ProjectStatus status)
    {
        return await _context.Projects
            .Include(p => p.Components)
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<DesignProject> DuplicateProjectAsync(Guid id, string newName)
    {
        var originalProject = await GetProjectByIdAsync(id);
        if (originalProject == null)
        {
            throw new ArgumentException("Project not found");
        }

        var duplicatedProject = new DesignProject
        {
            Id = Guid.NewGuid(),
            Name = newName,
            Description = originalProject.Description,
            Type = originalProject.Type,
            Status = ProjectStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = originalProject.CreatedBy,
            Settings = originalProject.Settings
        };

        // Duplicate components
        foreach (var component in originalProject.Components)
        {
            var duplicatedComponent = new DesignComponent
            {
                Id = Guid.NewGuid(),
                Name = component.Name,
                Description = component.Description,
                Type = component.Type,
                HtmlCode = component.HtmlCode,
                CssCode = component.CssCode,
                JsCode = component.JsCode,
                Style = component.Style,
                Accessibility = component.Accessibility,
                CreatedAt = DateTime.UtcNow,
                ProjectId = duplicatedProject.Id
            };

            duplicatedProject.Components.Add(duplicatedComponent);
        }

        await CreateProjectAsync(duplicatedProject);
        return duplicatedProject;
    }

    public async Task<ProjectSettings> UpdateProjectSettingsAsync(Guid projectId, ProjectSettings settings)
    {
        var project = await GetProjectByIdAsync(projectId);
        if (project == null)
        {
            throw new ArgumentException("Project not found");
        }

        project.Settings = settings;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return settings;
    }
} 