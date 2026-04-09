using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Project Tasks management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectTasksController : Base.BaseController
{
    private readonly IProjectTaskRepository _repository;
    private readonly ILogger<ProjectTasksController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ProjectTasksController(IProjectTaskRepository repository, ILogger<ProjectTasksController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all tasks
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tasks = await _repository.GetAllAsync();
            var taskDTOs = _mapper.Map<List<ProjectTaskDTO>>(tasks);
            return Ok(taskDTOs, "Tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks");
            return Error("An error occurred while retrieving tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get task by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var task = await _repository.GetDetailsByIdAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found");
            }
            var taskDTO = _mapper.Map<ProjectTaskDTO>(task);
            return Ok(taskDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving task {id}");
            return Error("An error occurred while retrieving the task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get tasks by project
    /// </summary>
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetByProject(int projectId)
    {
        try
        {
            var tasks = await _repository.GetTasksByProjectAsync(projectId);
            var taskDTOs = _mapper.Map<List<ProjectTaskDTO>>(tasks);
            return Ok(taskDTOs, "Tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving tasks for project {projectId}");
            return Error("An error occurred while retrieving tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get tasks by assignee
    /// </summary>
    [HttpGet("assignee/{resourceId}")]
    public async Task<IActionResult> GetByAssignee(int resourceId)
    {
        try
        {
            var tasks = await _repository.GetTasksByAssigneeAsync(resourceId);
            var taskDTOs = _mapper.Map<List<ProjectTaskDTO>>(tasks);
            return Ok(taskDTOs, "Tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving tasks for assignee {resourceId}");
            return Error("An error occurred while retrieving tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get tasks by type
    /// </summary>
    [HttpGet("type/{typeId}")]
    public async Task<IActionResult> GetByType(int typeId)
    {
        try
        {
            var tasks = await _repository.GetTasksByTypeAsync(typeId);
            var taskDTOs = _mapper.Map<List<ProjectTaskDTO>>(tasks);
            return Ok(taskDTOs, "Tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving tasks by type {typeId}");
            return Error("An error occurred while retrieving tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get overdue tasks
    /// </summary>
    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        try
        {
            var tasks = await _repository.GetOverdueTasksAsync();
            var taskDTOs = _mapper.Map<List<ProjectTaskDTO>>(tasks);
            return Ok(taskDTOs, "Overdue tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving overdue tasks");
            return Error("An error occurred while retrieving overdue tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get tasks by sprint
    /// </summary>
    [HttpGet("sprint/{sprintId}")]
    public async Task<IActionResult> GetBySprint(int sprintId)
    {
        try
        {
            var tasks = await _repository.GetTasksBySprint(sprintId);
            var taskDTOs = _mapper.Map<List<ProjectTaskDTO>>(tasks);
            return Ok(taskDTOs, "Tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving tasks for sprint {sprintId}");
            return Error("An error occurred while retrieving tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectTaskDTO createProjectTaskDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (string.IsNullOrWhiteSpace(createProjectTaskDTO.Title))
            {
                var validationErrors = new List<ApiError>
                {
                    new ApiError { Field = "title", Message = "Title is required" }
                };
                return ValidationError("Validation failed", validationErrors);
            }

            var task = _mapper.Map<ProjectTask>(createProjectTaskDTO);
            task.CreatedDate = DateTime.UtcNow;

            var createdTask = await _repository.CreateAsync(task);
            await _repository.SaveAsync();

            var createdTaskDTO = _mapper.Map<ProjectTaskDTO>(createdTask);
            return Created(nameof(GetById), "projecttask", new { id = createdTask.Id }, createdTaskDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return Error("An error occurred while creating the task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing task
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectTaskDTO updateProjectTaskDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingTask = await _repository.GetDetailsByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound($"Task with ID {id} not found");
            }

            existingTask.Title = updateProjectTaskDTO.Title ?? existingTask.Title;
            existingTask.Description = updateProjectTaskDTO.Description ?? existingTask.Description;
            existingTask.StartDate = updateProjectTaskDTO.StartDate ?? existingTask.StartDate;
            existingTask.EndDate = updateProjectTaskDTO.EndDate ?? existingTask.EndDate;
            existingTask.AssignTo = updateProjectTaskDTO.AssignTo ?? existingTask.AssignTo;
            existingTask.ProjectTaskTypeId = updateProjectTaskDTO.ProjectTaskTypeId ?? existingTask.ProjectTaskTypeId;
            existingTask.TotalTimeLogged = updateProjectTaskDTO.TotalTimeLogged ?? existingTask.TotalTimeLogged;

            var updatedTask = await _repository.EditAsync(existingTask);
            await _repository.SaveAsync();

            var updatedTaskDTO = _mapper.Map<ProjectTaskDTO>(updatedTask);
            return Ok(updatedTaskDTO, "Task updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating task {id}");
            return Error("An error occurred while updating the task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a task
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var task = await _repository.GetDetailsByIdAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting task {id}");
            return Error("An error occurred while deleting the task", StatusCodes.Status500InternalServerError);
        }
    }
}
