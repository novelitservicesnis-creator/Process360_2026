using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Project management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectsController : Base.BaseController
{
    private readonly IProjectRepository _repository;
    private readonly ILogger<ProjectsController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ProjectsController(IProjectRepository repository, ILogger<ProjectsController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all projects
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var projects = await _repository.GetAllAsync();
            var projectDTOs = _mapper.Map<List<ProjectDTO>>(projects);
            return Ok(projectDTOs, "Projects retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving projects");
            return Error("An error occurred while retrieving projects", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get project by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var project = await _repository.GetDetailsByIdAsync(id);
            if (project == null)
            {
                return NotFound($"Project with ID {id} not found");
            }
            var projectDTO = _mapper.Map<ProjectDTO>(project);
            return Ok(projectDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving project {id}");
            return Error("An error occurred while retrieving the project", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get project by code
    /// </summary>
    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        try
        {
            var project = await _repository.GetProjectByCodeAsync(code);
            if (project == null)
            {
                return NotFound($"Project with code '{code}' not found");
            }
            var projectDTO = _mapper.Map<ProjectDTO>(project);
            return Ok(projectDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving project by code {code}");
            return Error("An error occurred while retrieving the project", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get projects by customer
    /// </summary>
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        try
        {
            var projects = await _repository.GetProjectsByCustomerAsync(customerId);
            var projectDTOs = _mapper.Map<List<ProjectDTO>>(projects);
            return Ok(projectDTOs, "Projects retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving projects for customer {customerId}");
            return Error("An error occurred while retrieving projects", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get active projects
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var projects = await _repository.GetActiveProjectsAsync();
            var projectDTOs = _mapper.Map<List<ProjectDTO>>(projects);
            return Ok(projectDTOs, "Active projects retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active projects");
            return Error("An error occurred while retrieving active projects", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new project
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProjectDTO createProjectDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (string.IsNullOrWhiteSpace(createProjectDTO.Code) || string.IsNullOrWhiteSpace(createProjectDTO.Name))
            {
                var validationErrors = new List<ApiError>();
                if (string.IsNullOrWhiteSpace(createProjectDTO.Code))
                    validationErrors.Add(new ApiError { Field = "code", Message = "Code is required" });
                if (string.IsNullOrWhiteSpace(createProjectDTO.Name))
                    validationErrors.Add(new ApiError { Field = "name", Message = "Name is required" });

                return ValidationError("Validation failed", validationErrors);
            }

            var project = _mapper.Map<Project>(createProjectDTO);
            project.CreatedDate = DateTime.UtcNow;
            project.IsActive = true;

            var createdProject = await _repository.CreateAsync(project);
            await _repository.SaveAsync();

            var createdProjectDTO = _mapper.Map<ProjectDTO>(createdProject);
            return Created(nameof(GetById), "project", new { id = createdProject.Id }, createdProjectDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return Error("An error occurred while creating the project", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing project
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDTO updateProjectDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingProject = await _repository.GetDetailsByIdAsync(id);
            if (existingProject == null)
            {
                return NotFound($"Project with ID {id} not found");
            }

            existingProject.Code = updateProjectDTO.Code ?? existingProject.Code;
            existingProject.Name = updateProjectDTO.Name ?? existingProject.Name;
            existingProject.DatabaseSchema = updateProjectDTO.DatabaseSchema ?? existingProject.DatabaseSchema;
            existingProject.GitProvider = updateProjectDTO.GitProvider ?? existingProject.GitProvider;
            existingProject.GitRepoUrl = updateProjectDTO.GitRepoUrl ?? existingProject.GitRepoUrl;
            existingProject.IsActive = updateProjectDTO.IsActive ?? existingProject.IsActive;

            var updatedProject = await _repository.EditAsync(existingProject);
            await _repository.SaveAsync();

            var updatedProjectDTO = _mapper.Map<ProjectDTO>(updatedProject);
            return Ok(updatedProjectDTO, "Project updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating project {id}");
            return Error("An error occurred while updating the project", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a project
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var project = await _repository.GetDetailsByIdAsync(id);
            if (project == null)
            {
                return NotFound($"Project with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting project {id}");
            return Error("An error occurred while deleting the project", StatusCodes.Status500InternalServerError);
        }
    }
}
