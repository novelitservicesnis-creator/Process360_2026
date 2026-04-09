using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Task Comments management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class TaskCommentsController : Base.BaseController
{
    private readonly ITaskCommentsRepository _repository;
    private readonly ILogger<TaskCommentsController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public TaskCommentsController(ITaskCommentsRepository repository, ILogger<TaskCommentsController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all comments
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var comments = await _repository.GetAllAsync();
            var commentDTOs = _mapper.Map<List<TaskCommentsDTO>>(comments);
            return Ok(commentDTOs, "Comments retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving comments");
            return Error("An error occurred while retrieving comments", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get comment by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var comment = await _repository.GetDetailsByIdAsync(id);
            if (comment == null)
            {
                return NotFound($"Comment with ID {id} not found");
            }
            var commentDTO = _mapper.Map<TaskCommentsDTO>(comment);
            return Ok(commentDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving comment {id}");
            return Error("An error occurred while retrieving the comment", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get comments by task
    /// </summary>
    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetByTask(int taskId)
    {
        try
        {
            var comments = await _repository.GetCommentsByTaskAsync(taskId);
            var commentDTOs = _mapper.Map<List<TaskCommentsDTO>>(comments);
            return Ok(commentDTOs, "Comments retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving comments for task {taskId}");
            return Error("An error occurred while retrieving comments", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get comments by user
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        try
        {
            var comments = await _repository.GetCommentsByUserAsync(userId);
            var commentDTOs = _mapper.Map<List<TaskCommentsDTO>>(comments);
            return Ok(commentDTOs, "Comments retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving comments for user {userId}");
            return Error("An error occurred while retrieving comments", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new comment
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommentsDTO createCommentDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (createCommentDTO.ProjectTaskId <= 0)
            {
                var validationErrors = new List<ApiError>
                {
                    new ApiError { Field = "projectTaskId", Message = "Project Task ID is required" }
                };
                return ValidationError("Validation failed", validationErrors);
            }

            var comment = _mapper.Map<TaskComments>(createCommentDTO);
            comment.CreatedDate = DateTime.UtcNow;
            comment.CreatedBy = GetCurrentUserId() ?? 0;

            var createdComment = await _repository.CreateAsync(comment);
            await _repository.SaveAsync();

            var createdCommentDTO = _mapper.Map<TaskCommentsDTO>(createdComment);
            return Created(nameof(GetById), "taskcomment", new { id = createdComment.Id }, createdCommentDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating comment");
            return Error("An error occurred while creating the comment", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing comment
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskCommentsDTO updateCommentDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingComment = await _repository.GetDetailsByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound($"Comment with ID {id} not found");
            }

            existingComment.Comments = updateCommentDTO.Comment ?? existingComment.Comments;

            var updatedComment = await _repository.EditAsync(existingComment);
            await _repository.SaveAsync();

            var updatedCommentDTO = _mapper.Map<TaskCommentsDTO>(updatedComment);
            return Ok(updatedCommentDTO, "Comment updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating comment {id}");
            return Error("An error occurred while updating the comment", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a comment
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var comment = await _repository.GetDetailsByIdAsync(id);
            if (comment == null)
            {
                return NotFound($"Comment with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting comment {id}");
            return Error("An error occurred while deleting the comment", StatusCodes.Status500InternalServerError);
        }
    }
}
