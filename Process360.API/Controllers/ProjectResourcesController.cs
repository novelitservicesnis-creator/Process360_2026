using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Project Resources management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectResourcesController : Base.BaseController
{
    private readonly IProjectResourcesRepository _repository;
    private readonly ILogger<ProjectResourcesController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ProjectResourcesController(IProjectResourcesRepository repository, ILogger<ProjectResourcesController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all project resources
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var resources = await _repository.GetAllAsync();
            var resourceDTOs = _mapper.Map<List<ProjectResourcesDTO>>(resources);
            return Ok(resourceDTOs, "Project resources retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving project resources");
            return Error("An error occurred while retrieving project resources", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get project resource by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var resource = await _repository.GetDetailsByIdAsync(id);
            if (resource == null)
            {
                return NotFound($"Project resource with ID {id} not found");
            }
            var resourceDTO = _mapper.Map<ProjectResourcesDTO>(resource);
            return Ok(resourceDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving project resource {id}");
            return Error("An error occurred while retrieving the project resource", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get resources by project
    /// </summary>
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetByProject(int projectId)
    {
        try
        {
            var resources = await _repository.GetResourcesByProjectAsync(projectId);
            var resourceDTOs = _mapper.Map<List<ProjectResourcesDTO>>(resources);
            return Ok(resourceDTOs, "Project resources retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving resources for project {projectId}");
            return Error("An error occurred while retrieving project resources", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get projects by resource
    /// </summary>
    [HttpGet("resource/{resourceId}")]
    public async Task<IActionResult> GetByResource(int resourceId)
    {
        try
        {
            var resources = await _repository.GetProjectsByResourceAsync(resourceId);
            var resourceDTOs = _mapper.Map<List<ProjectResourcesDTO>>(resources);
            return Ok(resourceDTOs, "Project resources retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving projects for resource {resourceId}");
            return Error("An error occurred while retrieving project resources", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get resources by role
    /// </summary>
    [HttpGet("role/{role}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByRole(string role)
    {
        try
        {
            var resources = await _repository.GetResourcesByRoleAsync(role);
            var resourceDTOs = _mapper.Map<List<ProjectResourcesDTO>>(resources);
            return Ok(resourceDTOs, "Project resources retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving resources by role {role}");
            return Error("An error occurred while retrieving project resources", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new project resource
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProjectResourcesDTO createProjectResourcesDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (createProjectResourcesDTO.ResourceId <= 0 || createProjectResourcesDTO.ProjectId <= 0)
            {
                var validationErrors = new List<ApiError>();
                if (createProjectResourcesDTO.ResourceId <= 0)
                    validationErrors.Add(new ApiError { Field = "resourceId", Message = "Resource ID is required" });
                if (createProjectResourcesDTO.ProjectId <= 0)
                    validationErrors.Add(new ApiError { Field = "projectId", Message = "Project ID is required" });

                return ValidationError("Validation failed", validationErrors);
            }

            var projectResource = _mapper.Map<ProjectResources>(createProjectResourcesDTO);
            var createdResource = await _repository.CreateAsync(projectResource);
            await _repository.SaveAsync();

            var createdResourceDTO = _mapper.Map<ProjectResourcesDTO>(createdResource);
            return Created(nameof(GetById), "projectresource", new { id = createdResource.Id }, createdResourceDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project resource");
            return Error("An error occurred while creating the project resource", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing project resource
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectResourcesDTO updateProjectResourcesDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingResource = await _repository.GetDetailsByIdAsync(id);
            if (existingResource == null)
            {
                return NotFound($"Project resource with ID {id} not found");
            }

            existingResource.Role = updateProjectResourcesDTO.Role ?? existingResource.Role;

            var updatedResource = await _repository.EditAsync(existingResource);
            await _repository.SaveAsync();

            var updatedResourceDTO = _mapper.Map<ProjectResourcesDTO>(updatedResource);
            return Ok(updatedResourceDTO, "Project resource updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating project resource {id}");
            return Error("An error occurred while updating the project resource", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a project resource
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var resource = await _repository.GetDetailsByIdAsync(id);
            if (resource == null)
            {
                return NotFound($"Project resource with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting project resource {id}");
            return Error("An error occurred while deleting the project resource", StatusCodes.Status500InternalServerError);
        }
    }
}
