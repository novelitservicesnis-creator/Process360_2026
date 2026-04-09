using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Project Task Linked management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectTaskLinkedController : Base.BaseController
{
    private readonly IProjectTaskLinkedRepository _repository;
    private readonly ILogger<ProjectTaskLinkedController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ProjectTaskLinkedController(IProjectTaskLinkedRepository repository, ILogger<ProjectTaskLinkedController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all linked tasks
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var linkedTasks = await _repository.GetAllAsync();
            var linkedTaskDTOs = _mapper.Map<List<ProjectTaskLinkedDTO>>(linkedTasks);
            return Ok(linkedTaskDTOs, "Linked tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving linked tasks");
            return Error("An error occurred while retrieving linked tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get linked task by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var linkedTask = await _repository.GetDetailsByIdAsync(id);
            if (linkedTask == null)
            {
                return NotFound($"Linked task with ID {id} not found");
            }
            var linkedTaskDTO = _mapper.Map<ProjectTaskLinkedDTO>(linkedTask);
            return Ok(linkedTaskDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving linked task {id}");
            return Error("An error occurred while retrieving the linked task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get linked tasks by task
    /// </summary>
    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetByTask(int taskId)
    {
        try
        {
            var linkedTasks = await _repository.GetLinkedTasksByTaskAsync(taskId);
            var linkedTaskDTOs = _mapper.Map<List<ProjectTaskLinkedDTO>>(linkedTasks);
            return Ok(linkedTaskDTOs, "Linked tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving linked tasks for task {taskId}");
            return Error("An error occurred while retrieving linked tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get linked tasks by relation type
    /// </summary>
    [HttpGet("relation/{relationType}")]
    public async Task<IActionResult> GetByRelationType(string relationType)
    {
        try
        {
            var linkedTasks = await _repository.GetLinkedTasksByRelationTypeAsync(relationType);
            var linkedTaskDTOs = _mapper.Map<List<ProjectTaskLinkedDTO>>(linkedTasks);
            return Ok(linkedTaskDTOs, "Linked tasks retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving linked tasks by relation type {relationType}");
            return Error("An error occurred while retrieving linked tasks", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new linked task
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectTaskLinkedDTO createLinkedTaskDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (createLinkedTaskDTO.ProjectTaskId <= 0 || createLinkedTaskDTO.LinkedProjectTaskId <= 0)
            {
                var validationErrors = new List<ApiError>();
                if (createLinkedTaskDTO.ProjectTaskId <= 0)
                    validationErrors.Add(new ApiError { Field = "projectTaskId", Message = "Project Task ID is required" });
                if (createLinkedTaskDTO.LinkedProjectTaskId <= 0)
                    validationErrors.Add(new ApiError { Field = "linkedProjectTaskId", Message = "Linked Project Task ID is required" });

                return ValidationError("Validation failed", validationErrors);
            }

            var linkedTask = _mapper.Map<ProjectTaskLinked>(createLinkedTaskDTO);
            linkedTask.RelatedProjectTaskId = createLinkedTaskDTO.LinkedProjectTaskId;
            linkedTask.CreatedDate = DateTime.UtcNow;

            var createdLinkedTask = await _repository.CreateAsync(linkedTask);
            await _repository.SaveAsync();

            var createdLinkedTaskDTO = _mapper.Map<ProjectTaskLinkedDTO>(createdLinkedTask);
            return Created(nameof(GetById), "projecttasklinked", new { id = createdLinkedTask.Id }, createdLinkedTaskDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating linked task");
            return Error("An error occurred while creating the linked task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing linked task
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectTaskLinkedDTO updateLinkedTaskDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingLinkedTask = await _repository.GetDetailsByIdAsync(id);
            if (existingLinkedTask == null)
            {
                return NotFound($"Linked task with ID {id} not found");
            }

            existingLinkedTask.RelationType = updateLinkedTaskDTO.LinkType ?? existingLinkedTask.RelationType;

            var updatedLinkedTask = await _repository.EditAsync(existingLinkedTask);
            await _repository.SaveAsync();

            var updatedLinkedTaskDTO = _mapper.Map<ProjectTaskLinkedDTO>(updatedLinkedTask);
            return Ok(updatedLinkedTaskDTO, "Linked task updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating linked task {id}");
            return Error("An error occurred while updating the linked task", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a linked task
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var linkedTask = await _repository.GetDetailsByIdAsync(id);
            if (linkedTask == null)
            {
                return NotFound($"Linked task with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting linked task {id}");
            return Error("An error occurred while deleting the linked task", StatusCodes.Status500InternalServerError);
        }
    }
}
