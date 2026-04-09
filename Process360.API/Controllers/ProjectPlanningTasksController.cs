using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;

namespace Process360.API.Controllers;

/// <summary>
/// Project Planning Tasks management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectPlanningTasksController : Base.BaseController
{
    private readonly IProjectPlanningTasksRepository _repository;
    private readonly ILogger<ProjectPlanningTasksController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ProjectPlanningTasksController(IProjectPlanningTasksRepository repository, ILogger<ProjectPlanningTasksController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all planning tasks
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var planningTasks = await _repository.GetAllAsync();
            return Ok(planningTasks.ToList(), "Planning tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving planning tasks");
            return Error("An error occurred while retrieving planning tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get planning task by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var planningTask = await _repository.GetDetailsByIdAsync(id);
            if (planningTask == null)
            {
                return NotFound($"Planning task with ID {id} not found");
            }
            return Ok(planningTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving planning task {id}");
            return Error("An error occurred while retrieving the planning task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get tasks by planning
    /// </summary>
    [HttpGet("planning/{planningId}")]
    public async Task<IActionResult> GetByPlanning(int planningId)
    {
        try
        {
            var planningTasks = await _repository.GetTasksByPlanningAsync(planningId);
            return Ok(planningTasks.ToList(), "Planning tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving tasks for planning {planningId}");
            return Error("An error occurred while retrieving planning tasks", StatusCodes.Status500InternalServerError);
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
            var planningTasks = await _repository.GetTasksByProjectAsync(projectId);
            return Ok(planningTasks.ToList(), "Planning tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving tasks for project {projectId}");
            return Error("An error occurred while retrieving planning tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get completed tasks
    /// </summary>
    [HttpGet("completed")]
    public async Task<IActionResult> GetCompleted()
    {
        try
        {
            var planningTasks = await _repository.GetCompletedTasksAsync();
            return Ok(planningTasks.ToList(), "Completed planning tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving completed planning tasks");
            return Error("An error occurred while retrieving completed planning tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new planning task
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectPlanningTasks planningTask)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            planningTask.CreatedDate = DateTime.UtcNow;

            var createdPlanningTask = await _repository.CreateAsync(planningTask);
            await _repository.SaveAsync();

            return Created(nameof(GetById), "projectplanningtask", new { id = createdPlanningTask.Id }, createdPlanningTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating planning task");
            return Error("An error occurred while creating the planning task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing planning task
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectPlanningTasks planningTask)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingPlanningTask = await _repository.GetDetailsByIdAsync(id);
            if (existingPlanningTask == null)
            {
                return NotFound($"Planning task with ID {id} not found");
            }

            existingPlanningTask.IsCompleted = planningTask.IsCompleted ?? existingPlanningTask.IsCompleted;
            existingPlanningTask.ProjectId = planningTask.ProjectId ?? existingPlanningTask.ProjectId;
            existingPlanningTask.ProjectTaskId = planningTask.ProjectTaskId ?? existingPlanningTask.ProjectTaskId;

            var updatedPlanningTask = await _repository.EditAsync(existingPlanningTask);
            await _repository.SaveAsync();

            return Ok(updatedPlanningTask, "Planning task updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating planning task {id}");
            return Error("An error occurred while updating the planning task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a planning task
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var planningTask = await _repository.GetDetailsByIdAsync(id);
            if (planningTask == null)
            {
                return NotFound($"Planning task with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting planning task {id}");
            return Error("An error occurred while deleting the planning task", StatusCodes.Status500InternalServerError);
        }
    }
}
