using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Project Planning management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class ProjectPlanningsController : Base.BaseController
{
    private readonly IProjectPlanningRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjectPlanningsController> _logger;

    public ProjectPlanningsController(IProjectPlanningRepository repository, IMapper mapper, ILogger<ProjectPlanningsController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Get all plannings
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var plannings = await _repository.GetAllAsync();
            var planningDTOs = _mapper.Map<List<ProjectPlanningDTO>>(plannings);
            return Ok(planningDTOs, "Plannings retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving plannings");
            return Error("An error occurred while retrieving plannings", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get planning by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var planning = await _repository.GetDetailsByIdAsync(id);
            if (planning == null)
            {
                return NotFound($"Planning with ID {id} not found");
            }
            var planningDTO = _mapper.Map<ProjectPlanningDTO>(planning);
            return Ok(planningDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving planning {id}");
            return Error("An error occurred while retrieving the planning", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get plannings by date range
    /// </summary>
    [HttpGet("date-range")]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var plannings = await _repository.GetPlanningByDateRangeAsync(startDate, endDate);
            var planningDTOs = _mapper.Map<List<ProjectPlanningDTO>>(plannings);
            return Ok(planningDTOs, "Plannings retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving plannings by date range");
            return Error("An error occurred while retrieving plannings", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get current plannings
    /// </summary>
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent()
    {
        try
        {
            var plannings = await _repository.GetCurrentPlanningsAsync();
            var planningDTOs = _mapper.Map<List<ProjectPlanningDTO>>(plannings);
            return Ok(planningDTOs, "Current plannings retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current plannings");
            return Error("An error occurred while retrieving current plannings", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new planning
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProjectPlanningDTO createProjectPlanningDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var planning = _mapper.Map<ProjectPlanning>(createProjectPlanningDTO);
            planning.CreatedDate = DateTime.UtcNow;

            var createdPlanning = await _repository.CreateAsync(planning);
            await _repository.SaveAsync();

            var createdPlanningDTO = _mapper.Map<ProjectPlanningDTO>(createdPlanning);
            return Created(nameof(GetById), "projectplanning", new { id = createdPlanning.Id }, createdPlanningDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating planning");
            return Error("An error occurred while creating the planning", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing planning
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectPlanningDTO updateProjectPlanningDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingPlanning = await _repository.GetDetailsByIdAsync(id);
            if (existingPlanning == null)
            {
                return NotFound($"Planning with ID {id} not found");
            }

            existingPlanning.Name = updateProjectPlanningDTO.Name ?? existingPlanning.Name;
            existingPlanning.Goal = updateProjectPlanningDTO.Goal ?? existingPlanning.Goal;
            existingPlanning.StartDate = updateProjectPlanningDTO.StartDate ?? existingPlanning.StartDate;
            existingPlanning.EndDate = updateProjectPlanningDTO.EndDate ?? existingPlanning.EndDate;

            var updatedPlanning = await _repository.EditAsync(existingPlanning);
            await _repository.SaveAsync();

            var updatedPlanningDTO = _mapper.Map<ProjectPlanningDTO>(updatedPlanning);
            return Ok(updatedPlanningDTO, "Planning updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating planning {id}");
            return Error("An error occurred while updating the planning", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a planning
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var planning = await _repository.GetDetailsByIdAsync(id);
            if (planning == null)
            {
                return NotFound($"Planning with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting planning {id}");
            return Error("An error occurred while deleting the planning", StatusCodes.Status500InternalServerError);
        }
    }
}
