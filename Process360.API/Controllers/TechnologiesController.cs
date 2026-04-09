using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Technologies management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class TechnologiesController : Base.BaseController
{
    private readonly ITechnologyRepository _repository;
    private readonly ILogger<TechnologiesController> _logger;
    private readonly AutoMapper.IMapper _mapper;

    public TechnologiesController(ITechnologyRepository repository, ILogger<TechnologiesController> logger, AutoMapper.IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all technologies
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var technologies = await _repository.GetAllAsync();
            var technologyDTOs = _mapper.Map<List<TechnologyDTO>>(technologies);
            return Ok(technologyDTOs, "Technologies retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving technologies");
            return Error("An error occurred while retrieving technologies", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get technology by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var technology = await _repository.GetDetailsByIdAsync(id);
            if (technology == null)
            {
                return NotFound($"Technology with ID {id} not found");
            }
            var technologyDTO = _mapper.Map<TechnologyDTO>(technology);
            return Ok(technologyDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving technology {id}");
            return Error("An error occurred while retrieving the technology", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get technologies by type
    /// </summary>
    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetByType(string type)
    {
        try
        {
            var technologies = await _repository.GetTechnologiesByTypeAsync(type);
            var technologyDTOs = _mapper.Map<List<TechnologyDTO>>(technologies);
            return Ok(technologyDTOs, "Technologies retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving technologies by type {type}");
            return Error("An error occurred while retrieving technologies", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get technology by name
    /// </summary>
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        try
        {
            var technology = await _repository.GetTechnologyByNameAsync(name);
            if (technology == null)
            {
                return NotFound($"Technology '{name}' not found");
            }
            var technologyDTO = _mapper.Map<TechnologyDTO>(technology);
            return Ok(technologyDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving technology by name {name}");
            return Error("An error occurred while retrieving the technology", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get active technologies
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var technologies = await _repository.GetActiveTechnologiesAsync();
            var technologyDTOs = _mapper.Map<List<TechnologyDTO>>(technologies);
            return Ok(technologyDTOs, "Active technologies retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active technologies");
            return Error("An error occurred while retrieving active technologies", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new technology
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateTechnologyDTO createTechnologyDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (string.IsNullOrWhiteSpace(createTechnologyDTO.TechnologyName))
            {
                var validationErrors = new List<ApiError>
                {
                    new ApiError { Field = "technologyName", Message = "Technology name is required" }
                };
                return ValidationError("Validation failed", validationErrors);
            }

            var technology = _mapper.Map<Technology>(createTechnologyDTO);
            technology.CreatedDate = DateTime.UtcNow;
            technology.IsActive = true;

            var createdTechnology = await _repository.CreateAsync(technology);
            await _repository.SaveAsync();

            var createdTechnologyDTO = _mapper.Map<TechnologyDTO>(createdTechnology);
            return Created(nameof(GetById), "technology", new { id = createdTechnology.Id }, createdTechnologyDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating technology");
            return Error("An error occurred while creating the technology", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing technology
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTechnologyDTO updateTechnologyDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingTechnology = await _repository.GetDetailsByIdAsync(id);
            if (existingTechnology == null)
            {
                return NotFound($"Technology with ID {id} not found");
            }

            existingTechnology.Name = updateTechnologyDTO.TechnologyName ?? existingTechnology.Name;

            var updatedTechnology = await _repository.EditAsync(existingTechnology);
            await _repository.SaveAsync();

            var updatedTechnologyDTO = _mapper.Map<TechnologyDTO>(updatedTechnology);
            return Ok(updatedTechnologyDTO, "Technology updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating technology {id}");
            return Error("An error occurred while updating the technology", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a technology
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var technology = await _repository.GetDetailsByIdAsync(id);
            if (technology == null)
            {
                return NotFound($"Technology with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting technology {id}");
            return Error("An error occurred while deleting the technology", StatusCodes.Status500InternalServerError);
        }
    }
}
