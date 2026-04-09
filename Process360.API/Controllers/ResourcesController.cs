using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Resources/Team Members management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ResourcesController : Base.BaseController
{
    private readonly IResourcesRepository _repository;
    private readonly ILogger<ResourcesController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public ResourcesController(IResourcesRepository repository, ILogger<ResourcesController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all resources
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var resources = await _repository.GetAllAsync();
            var resourceDTOs = _mapper.Map<List<ResourcesDTO>>(resources);
            return Ok(resourceDTOs, "Resources retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving resources");
            return Error("An error occurred while retrieving resources", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get resource by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var resource = await _repository.GetDetailsByIdAsync(id);
            if (resource == null)
            {
                return NotFound($"Resource with ID {id} not found");
            }
            var resourceDTO = _mapper.Map<ResourcesDTO>(resource);
            return Ok(resourceDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving resource {id}");
            return Error("An error occurred while retrieving the resource", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get resource by email
    /// </summary>
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            var resource = await _repository.GetResourceByEmailAsync(email);
            if (resource == null)
            {
                return NotFound($"Resource with email '{email}' not found");
            }
            var resourceDTO = _mapper.Map<ResourcesDTO>(resource);
            return Ok(resourceDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving resource by email {email}");
            return Error("An error occurred while retrieving the resource", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get active resources
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var resources = await _repository.GetActiveResourcesAsync();
            var resourceDTOs = _mapper.Map<List<ResourcesDTO>>(resources);
            return Ok(resourceDTOs, "Active resources retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active resources");
            return Error("An error occurred while retrieving active resources", StatusCodes.Status500InternalServerError);
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
            var resourceDTOs = _mapper.Map<List<ResourcesDTO>>(resources);
            return Ok(resourceDTOs, "Resources retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving resources by role {role}");
            return Error("An error occurred while retrieving resources", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new resource
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateResourcesDTO createResourcesDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (string.IsNullOrWhiteSpace(createResourcesDTO.Email) || string.IsNullOrWhiteSpace(createResourcesDTO.Password))
            {
                var validationErrors = new List<ApiError>();
                if (string.IsNullOrWhiteSpace(createResourcesDTO.Email))
                    validationErrors.Add(new ApiError { Field = "email", Message = "Email is required" });
                if (string.IsNullOrWhiteSpace(createResourcesDTO.Password))
                    validationErrors.Add(new ApiError { Field = "password", Message = "Password is required" });

                return ValidationError("Validation failed", validationErrors);
            }

            var resource = _mapper.Map<Resources>(createResourcesDTO);
            resource.CreatedDate = DateTime.UtcNow;
            resource.IsActive = true;

            var createdResource = await _repository.CreateAsync(resource);
            await _repository.SaveAsync();

            var createdResourceDTO = _mapper.Map<ResourcesDTO>(createdResource);
            return Created(nameof(GetById), "resource", new { id = createdResource.Id }, createdResourceDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating resource");
            return Error("An error occurred while creating the resource", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing resource
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateResourcesDTO updateResourcesDTO)
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
                return NotFound($"Resource with ID {id} not found");
            }

            existingResource.FirstName = updateResourcesDTO.FirstName ?? existingResource.FirstName;
            existingResource.LastName = updateResourcesDTO.LastName ?? existingResource.LastName;
            existingResource.Email = updateResourcesDTO.Email ?? existingResource.Email;
            existingResource.Role = updateResourcesDTO.Role ?? existingResource.Role;
            existingResource.Address = updateResourcesDTO.Address ?? existingResource.Address;
            existingResource.CurrentLocation = updateResourcesDTO.CurrentLocation ?? existingResource.CurrentLocation;
            existingResource.Experience = updateResourcesDTO.Experience ?? existingResource.Experience;
            existingResource.IsActive = updateResourcesDTO.IsActive ?? existingResource.IsActive;

            var updatedResource = await _repository.EditAsync(existingResource);
            await _repository.SaveAsync();

            var updatedResourceDTO = _mapper.Map<ResourcesDTO>(updatedResource);
            return Ok(updatedResourceDTO, "Resource updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating resource {id}");
            return Error("An error occurred while updating the resource", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a resource
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
                return NotFound($"Resource with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting resource {id}");
            return Error("An error occurred while deleting the resource", StatusCodes.Status500InternalServerError);
        }
    }
}
