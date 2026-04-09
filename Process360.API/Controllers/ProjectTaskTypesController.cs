using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Project Task Types management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectTaskTypesController : Base.BaseController
{
    private readonly IProjectTaskTypeRepository _repository;
    private readonly ILogger<ProjectTaskTypesController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ProjectTaskTypesController(IProjectTaskTypeRepository repository, ILogger<ProjectTaskTypesController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all task types
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var taskTypes = await _repository.GetAllAsync();
            var taskTypeDTOs = _mapper.Map<List<ProjectTaskTypeDTO>>(taskTypes);
            return Ok(taskTypeDTOs, "Task types retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task types");
            return Error("An error occurred while retrieving task types", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get task type by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var taskType = await _repository.GetDetailsByIdAsync(id);
            if (taskType == null)
            {
                return NotFound($"Task type with ID {id} not found");
            }
            var taskTypeDTO = _mapper.Map<ProjectTaskTypeDTO>(taskType);
            return Ok(taskTypeDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving task type {id}");
            return Error("An error occurred while retrieving the task type", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get task type by name
    /// </summary>
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        try
        {
            var taskType = await _repository.GetTaskTypeByNameAsync(name);
            if (taskType == null)
            {
                return NotFound($"Task type '{name}' not found");
            }
            var taskTypeDTO = _mapper.Map<ProjectTaskTypeDTO>(taskType);
            return Ok(taskTypeDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving task type by name {name}");
            return Error("An error occurred while retrieving the task type", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new task type
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProjectTaskTypeDTO createProjectTaskTypeDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (string.IsNullOrWhiteSpace(createProjectTaskTypeDTO.Name))
            {
                var validationErrors = new List<ApiError>
                {
                    new ApiError { Field = "name", Message = "Name is required" }
                };
                return ValidationError("Validation failed", validationErrors);
            }

            var taskType = _mapper.Map<ProjectTaskType>(createProjectTaskTypeDTO);
            taskType.CreatedDate = DateTime.UtcNow;

            var createdTaskType = await _repository.CreateAsync(taskType);
            await _repository.SaveAsync();

            var createdTaskTypeDTO = _mapper.Map<ProjectTaskTypeDTO>(createdTaskType);
            return Created(nameof(GetById), "projecttasktype", new { id = createdTaskType.Id }, createdTaskTypeDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task type");
            return Error("An error occurred while creating the task type", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing task type
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectTaskTypeDTO updateProjectTaskTypeDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingTaskType = await _repository.GetDetailsByIdAsync(id);
            if (existingTaskType == null)
            {
                return NotFound($"Task type with ID {id} not found");
            }

            existingTaskType.Name = updateProjectTaskTypeDTO.Name ?? existingTaskType.Name;

            var updatedTaskType = await _repository.EditAsync(existingTaskType);
            await _repository.SaveAsync();

            var updatedTaskTypeDTO = _mapper.Map<ProjectTaskTypeDTO>(updatedTaskType);
            return Ok(updatedTaskTypeDTO, "Task type updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating task type {id}");
            return Error("An error occurred while updating the task type", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a task type
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var taskType = await _repository.GetDetailsByIdAsync(id);
            if (taskType == null)
            {
                return NotFound($"Task type with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting task type {id}");
            return Error("An error occurred while deleting the task type", StatusCodes.Status500InternalServerError);
        }
    }
}
