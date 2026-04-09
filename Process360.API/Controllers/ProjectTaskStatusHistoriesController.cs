using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Project Task Status History management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectTaskStatusHistoriesController : Base.BaseController
{
    private readonly IProjectTaskStatusHistoryRepository _repository;
    private readonly ILogger<ProjectTaskStatusHistoriesController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ProjectTaskStatusHistoriesController(IProjectTaskStatusHistoryRepository repository, ILogger<ProjectTaskStatusHistoriesController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all status histories
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var histories = await _repository.GetAllAsync();
            var historyDTOs = _mapper.Map<List<ProjectTaskStatusHistoryDTO>>(histories);
            return Ok(historyDTOs, "Status histories retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving status histories");
            return Error("An error occurred while retrieving status histories", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get status history by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var history = await _repository.GetDetailsByIdAsync(id);
            if (history == null)
            {
                return NotFound($"Status history with ID {id} not found");
            }
            var historyDTO = _mapper.Map<ProjectTaskStatusHistoryDTO>(history);
            return Ok(historyDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving status history {id}");
            return Error("An error occurred while retrieving the status history", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get status history by task
    /// </summary>
    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetByTask(int taskId)
    {
        try
        {
            var histories = await _repository.GetStatusHistoryByTaskAsync(taskId);
            var historyDTOs = _mapper.Map<List<ProjectTaskStatusHistoryDTO>>(histories);
            return Ok(historyDTOs, "Status histories retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving status history for task {taskId}");
            return Error("An error occurred while retrieving status histories", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new status history
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectTaskStatusHistoryDTO createHistoryDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (createHistoryDTO.ProjectTaskId <= 0)
            {
                var validationErrors = new List<ApiError>
                {
                    new ApiError { Field = "projectTaskId", Message = "Project Task ID is required" }
                };
                return ValidationError("Validation failed", validationErrors);
            }

            var history = _mapper.Map<ProjectTaskStatusHistory>(createHistoryDTO);
            history.CreatedDate = DateTime.UtcNow;

            var createdHistory = await _repository.CreateAsync(history);
            await _repository.SaveAsync();

            var createdHistoryDTO = _mapper.Map<ProjectTaskStatusHistoryDTO>(createdHistory);
            return Created(nameof(GetById), "projecttaskstatushistory", new { id = createdHistory.Id }, createdHistoryDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating status history");
            return Error("An error occurred while creating the status history", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a status history
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var history = await _repository.GetDetailsByIdAsync(id);
            if (history == null)
            {
                return NotFound($"Status history with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting status history {id}");
            return Error("An error occurred while deleting the status history", StatusCodes.Status500InternalServerError);
        }
    }
}
