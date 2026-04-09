using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers;

/// <summary>
/// Customer management endpoints
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class CustomersController : Base.BaseController
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerRepository repository, IMapper mapper, ILogger<CustomersController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Get all customers
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var customers = await _repository.GetAllAsync();
            var customerDTOs = _mapper.Map<List<CustomerDTO>>(customers);
            return Ok(customerDTOs, "Customers retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customers");
            return Error("An error occurred while retrieving customers", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var customer = await _repository.GetDetailsByIdAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found");
            }
            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return Ok(customerDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving customer {id}");
            return Error("An error occurred while retrieving the customer", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get customer by login
    /// </summary>
    [HttpGet("login/{login}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByLogin(string login)
    {
        try
        {
            var customer = await _repository.GetCustomerByLoginAsync(login);
            if (customer == null)
            {
                return NotFound($"Customer with login '{login}' not found");
            }
            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return Ok(customerDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving customer by login {login}");
            return Error("An error occurred while retrieving the customer", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get customer by email
    /// </summary>
    [HttpGet("email/{email}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            var customer = await _repository.GetCustomerByEmailAsync(email);
            if (customer == null)
            {
                return NotFound($"Customer with email '{email}' not found");
            }
            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return Ok(customerDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving customer by email {email}");
            return Error("An error occurred while retrieving the customer", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get active customers
    /// </summary>
    [HttpGet("active")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var customers = await _repository.GetActiveCustomersAsync();
            var customerDTOs = _mapper.Map<List<CustomerDTO>>(customers);
            return Ok(customerDTOs, "Active customers retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active customers");
            return Error("An error occurred while retrieving active customers", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDTO createCustomerDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            if (string.IsNullOrWhiteSpace(createCustomerDTO.Login) || string.IsNullOrWhiteSpace(createCustomerDTO.Name))
            {
                var validationErrors = new List<ApiError>();
                if (string.IsNullOrWhiteSpace(createCustomerDTO.Login))
                    validationErrors.Add(new ApiError { Field = "login", Message = "Login is required" });
                if (string.IsNullOrWhiteSpace(createCustomerDTO.Name))
                    validationErrors.Add(new ApiError { Field = "name", Message = "Name is required" });

                return ValidationError("Validation failed", validationErrors);
            }

            var customer = _mapper.Map<Customer>(createCustomerDTO);
            customer.CreatedDate = DateTime.UtcNow;
            customer.IsActive = true;

            var createdCustomer = await _repository.CreateAsync(customer);
            await _repository.SaveAsync();

            var createdCustomerDTO = _mapper.Map<CustomerDTO>(createdCustomer);
            return Created(nameof(GetById), "customer", new { id = createdCustomer.Id }, createdCustomerDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return Error("An error occurred while creating the customer", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDTO updateCustomerDTO)
    {
        try
        {
            if (!ValidateModel(out var errors))
            {
                return ValidationError("Validation failed", errors);
            }

            var existingCustomer = await _repository.GetDetailsByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound($"Customer with ID {id} not found");
            }

            // Map DTO properties to existing customer (partial update)
            existingCustomer.Login = updateCustomerDTO.Login ?? existingCustomer.Login;
            existingCustomer.Name = updateCustomerDTO.Name ?? existingCustomer.Name;
            existingCustomer.Email = updateCustomerDTO.Email ?? existingCustomer.Email;
            existingCustomer.Website = updateCustomerDTO.Website ?? existingCustomer.Website;
            existingCustomer.Company = updateCustomerDTO.Company ?? existingCustomer.Company;
            existingCustomer.IsActive = updateCustomerDTO.IsActive ?? existingCustomer.IsActive;

            var updatedCustomer = await _repository.EditAsync(existingCustomer);
            await _repository.SaveAsync();

            var updatedCustomerDTO = _mapper.Map<CustomerDTO>(updatedCustomer);
            return Ok(updatedCustomerDTO, "Customer updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating customer {id}");
            return Error("An error occurred while updating the customer", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete a customer
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var customer = await _repository.GetDetailsByIdAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found");
            }

            await _repository.DeleteAsync(id);
            await _repository.SaveAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting customer {id}");
            return Error("An error occurred while deleting the customer", StatusCodes.Status500InternalServerError);
        }
    }
}
