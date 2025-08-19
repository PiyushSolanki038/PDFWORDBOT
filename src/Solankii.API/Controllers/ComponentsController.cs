using Microsoft.AspNetCore.Mvc;
using Solankii.Core.Entities;
using Solankii.Core.Interfaces;

namespace Solankii.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComponentsController : ControllerBase
{
    private readonly IComponentService _componentService;
    private readonly IAIService _aiService;

    public ComponentsController(IComponentService componentService, IAIService aiService)
    {
        _componentService = componentService;
        _aiService = aiService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DesignComponent>>> GetComponents([FromQuery] Guid? projectId = null, [FromQuery] ComponentType? type = null)
    {
        try
        {
            List<DesignComponent> components;
            
            if (projectId.HasValue)
            {
                components = await _componentService.GetComponentsByProjectAsync(projectId.Value);
            }
            else if (type.HasValue)
            {
                components = await _componentService.GetComponentsByTypeAsync(type.Value);
            }
            else
            {
                return BadRequest("Either projectId or type must be specified");
            }
            
            return Ok(components);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DesignComponent>> GetComponent(Guid id)
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
            {
                return NotFound();
            }
            return Ok(component);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<DesignComponent>>> SearchComponents([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term is required");
            }

            var components = await _componentService.SearchComponentsAsync(searchTerm);
            return Ok(components);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<DesignComponent>> CreateComponent([FromBody] DesignComponent component)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            component.Id = Guid.NewGuid();
            component.CreatedAt = DateTime.UtcNow;

            var createdComponent = await _componentService.CreateComponentAsync(component);
            return CreatedAtAction(nameof(GetComponent), new { id = createdComponent.Id }, createdComponent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("generate")]
    public async Task<ActionResult<DesignComponent>> GenerateComponent([FromBody] GenerateComponentRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var component = await _aiService.GenerateComponentAsync(request.Description, request.Type, request.Style);
            component.ProjectId = request.ProjectId;
            component.CreatedAt = DateTime.UtcNow;

            var createdComponent = await _componentService.CreateComponentAsync(component);
            return CreatedAtAction(nameof(GetComponent), new { id = createdComponent.Id }, createdComponent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DesignComponent>> UpdateComponent(Guid id, [FromBody] DesignComponent component)
    {
        try
        {
            if (id != component.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            component.UpdatedAt = DateTime.UtcNow;
            var updatedComponent = await _componentService.UpdateComponentAsync(component);
            return Ok(updatedComponent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComponent(Guid id)
    {
        try
        {
            var success = await _componentService.DeleteComponentAsync(id);
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
    public async Task<ActionResult<DesignComponent>> DuplicateComponent(Guid id, [FromBody] string newName)
    {
        try
        {
            var duplicatedComponent = await _componentService.DuplicateComponentAsync(id, newName);
            return CreatedAtAction(nameof(GetComponent), new { id = duplicatedComponent.Id }, duplicatedComponent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/variants")]
    public async Task<ActionResult<List<ComponentVariant>>> GenerateVariants(Guid id)
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
            {
                return NotFound("Component not found");
            }

            var variants = await _componentService.GenerateVariantsAsync(component);
            return Ok(variants);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/export")]
    public async Task<ActionResult<string>> ExportComponent(Guid id, [FromQuery] string format = "React")
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
            {
                return NotFound("Component not found");
            }

            var exportedCode = await _componentService.ExportComponentAsync(component, format);
            return Ok(new { format, code = exportedCode });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("import")]
    public async Task<ActionResult<DesignComponent>> ImportComponent([FromBody] ImportComponentRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var component = await _componentService.ImportComponentAsync(request.Code, request.Format, request.ProjectId);
            return CreatedAtAction(nameof(GetComponent), new { id = component.Id }, component);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/optimize")]
    public async Task<ActionResult<DesignComponent>> OptimizeComponent(Guid id)
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
            {
                return NotFound("Component not found");
            }

            var optimizedComponent = await _componentService.OptimizeComponentAsync(component);
            return Ok(optimizedComponent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/accessibility")]
    public async Task<ActionResult<string>> GenerateAccessibilityImprovements(Guid id)
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
            {
                return NotFound("Component not found");
            }

            var improvedHtml = await _aiService.GenerateAccessibilityImprovementsAsync(component);
            return Ok(new { improvedHtml });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/responsive")]
    public async Task<ActionResult<string>> GenerateResponsiveCode(Guid id, [FromQuery] string framework = "CSS")
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
            {
                return NotFound("Component not found");
            }

            var responsiveCode = await _aiService.GenerateResponsiveCodeAsync(component.HtmlCode, framework);
            return Ok(new { framework, responsiveCode });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/analyze")]
    public async Task<ActionResult<object>> AnalyzeComponent(Guid id)
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
            {
                return NotFound("Component not found");
            }

            var usabilityScore = await _aiService.AnalyzeUsabilityScoreAsync(component);
            var accessibilityScore = await _aiService.AnalyzeAccessibilityScoreAsync(component);
            var performanceScore = await _aiService.AnalyzePerformanceScoreAsync(component);

            return Ok(new
            {
                componentId = id,
                scores = new
                {
                    usability = usabilityScore,
                    accessibility = accessibilityScore,
                    performance = performanceScore,
                    overall = (usabilityScore + accessibilityScore + performanceScore) / 3.0
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/documentation")]
    public async Task<ActionResult<string>> GenerateDocumentation(Guid id)
    {
        try
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null)
            {
                return NotFound("Component not found");
            }

            var documentation = await _aiService.GenerateDocumentationAsync(component);
            return Ok(new { documentation });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

public class GenerateComponentRequest
{
    public string Description { get; set; } = string.Empty;
    public ComponentType Type { get; set; }
    public ComponentStyle Style { get; set; } = new();
    public Guid ProjectId { get; set; }
}

public class ImportComponentRequest
{
    public string Code { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
} 