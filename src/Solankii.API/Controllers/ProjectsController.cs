using Microsoft.AspNetCore.Mvc;
using Solankii.Core.Entities;
using Solankii.Core.Interfaces;

namespace Solankii.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IAIService _aiService;

    public ProjectsController(IProjectService projectService, IAIService aiService)
    {
        _projectService = projectService;
        _aiService = aiService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DesignProject>>> GetProjects()
    {
        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DesignProject>> GetProject(Guid id)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<List<DesignProject>>> GetProjectsByType(ProjectType type)
    {
        try
        {
            var projects = await _projectService.GetProjectsByTypeAsync(type);
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<List<DesignProject>>> GetProjectsByStatus(ProjectStatus status)
    {
        try
        {
            var projects = await _projectService.GetProjectsByStatusAsync(status);
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<DesignProject>> CreateProject([FromBody] DesignProject project)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            project.Id = Guid.NewGuid();
            project.CreatedAt = DateTime.UtcNow;
            project.Status = ProjectStatus.Draft;

            var createdProject = await _projectService.CreateProjectAsync(project);
            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DesignProject>> UpdateProject(Guid id, [FromBody] DesignProject project)
    {
        try
        {
            if (id != project.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            project.UpdatedAt = DateTime.UtcNow;
            var updatedProject = await _projectService.UpdateProjectAsync(project);
            return Ok(updatedProject);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(Guid id)
    {
        try
        {
            var success = await _projectService.DeleteProjectAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/duplicate")]
    public async Task<ActionResult<DesignProject>> DuplicateProject(Guid id, [FromBody] string newName)
    {
        try
        {
            var duplicatedProject = await _projectService.DuplicateProjectAsync(id, newName);
            return CreatedAtAction(nameof(GetProject), new { id = duplicatedProject.Id }, duplicatedProject);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}/settings")]
    public async Task<ActionResult<ProjectSettings>> UpdateProjectSettings(Guid id, [FromBody] ProjectSettings settings)
    {
        try
        {
            var updatedSettings = await _projectService.UpdateProjectSettingsAsync(id, settings);
            return Ok(updatedSettings);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/suggestions")]
    public async Task<ActionResult<List<DesignSuggestion>>> GenerateSuggestions(Guid id, [FromQuery] Guid? componentId = null)
    {
        try
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound("Project not found");
            }

            DesignComponent? component = null;
            if (componentId.HasValue)
            {
                component = project.Components.FirstOrDefault(c => c.Id == componentId.Value);
            }

            var suggestions = await _aiService.GenerateDesignSuggestionsAsync(project, component);
            return Ok(suggestions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
} 