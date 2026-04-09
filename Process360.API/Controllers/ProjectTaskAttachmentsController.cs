using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Project Task Attachments management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectTaskAttachmentsController : Base.BaseController
{
    private readonly IProjectTaskAttachmentsRepository _repository;
    private readonly ILogger<ProjectTaskAttachmentsController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ProjectTaskAttachmentsController(IProjectTaskAttachmentsRepository repository, ILogger<ProjectTaskAttachmentsController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all attachments
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var attachments = await _repository.GetAllAsync();
            var attachmentDTOs = _mapper.Map<List<ProjectTaskAttachmentsDTO>>(attachments);
            return Ok(attachmentDTOs, "Attachments retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving attachments");
            return Error("An error occurred while retrieving attachments", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get attachment by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var attachment = await _repository.GetDetailsByIdAsync(id);
            if (attachment == null)
            {
                return NotFound($"Attachment with ID {id} not found");
            }
            var attachmentDTO = _mapper.Map<ProjectTaskAttachmentsDTO>(attachment);
            return Ok(attachmentDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving attachment {id}");
            return Error("An error occurred while retrieving the attachment", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get attachments by task
    /// </summary>
    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetByTask(int taskId)
    {
        try
        {
            var attachments = await _repository.GetAttachmentsByTaskAsync(taskId);
            var attachmentDTOs = _mapper.Map<List<ProjectTaskAttachmentsDTO>>(attachments);
            return Ok(attachmentDTOs, "Attachments retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving attachments for task {taskId}");
            return Error("An error occurred while retrieving attachments", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new attachment
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectTaskAttachmentsDTO createAttachmentDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (createAttachmentDTO.ProjectTaskId <= 0)
            {
                var validationErrors = new List<ApiError>
                {
                    new ApiError { Field = "projectTaskId", Message = "Project Task ID is required" }
                };
                return ValidationError("Validation failed", validationErrors);
            }

            var attachment = _mapper.Map<ProjectTaskAttachments>(createAttachmentDTO);
            attachment.CreatedDate = DateTime.UtcNow;

            var createdAttachment = await _repository.CreateAsync(attachment);
            await _repository.SaveAsync();

            var createdAttachmentDTO = _mapper.Map<ProjectTaskAttachmentsDTO>(createdAttachment);
            return Created(nameof(GetById), "projecttaskattachment", new { id = createdAttachment.Id }, createdAttachmentDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating attachment");
            return Error("An error occurred while creating the attachment", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing attachment
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectTaskAttachmentsDTO updateAttachmentDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingAttachment = await _repository.GetDetailsByIdAsync(id);
            if (existingAttachment == null)
            {
                return NotFound($"Attachment with ID {id} not found");
            }

            existingAttachment.FileName = updateAttachmentDTO.FileName ?? existingAttachment.FileName;
            existingAttachment.FileUrl = updateAttachmentDTO.FileUrl ?? existingAttachment.FileUrl;
            existingAttachment.FileType = updateAttachmentDTO.FileType ?? existingAttachment.FileType;
            existingAttachment.FileSize = updateAttachmentDTO.FileSize ?? existingAttachment.FileSize;

            var updatedAttachment = await _repository.EditAsync(existingAttachment);
            await _repository.SaveAsync();

            var updatedAttachmentDTO = _mapper.Map<ProjectTaskAttachmentsDTO>(updatedAttachment);
            return Ok(updatedAttachmentDTO, "Attachment updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating attachment {id}");
            return Error("An error occurred while updating the attachment", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete an attachment
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var attachment = await _repository.GetDetailsByIdAsync(id);
            if (attachment == null)
            {
                return NotFound($"Attachment with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting attachment {id}");
            return Error("An error occurred while deleting the attachment", StatusCodes.Status500InternalServerError);
        }
    }
}
