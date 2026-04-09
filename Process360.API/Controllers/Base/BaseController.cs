using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Process360.Repository.ViewModel;

namespace Process360.API.Controllers.Base;


public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Standard success response with data
    /// </summary>
    protected IActionResult Ok<T>(T data, string message = "Operation completed successfully")
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = new List<ApiError>(),
            Timestamp = DateTime.UtcNow
        };
        return base.Ok(response);
    }

    /// <summary>
    /// Standard success response for creation
    /// </summary>
    protected IActionResult Created<T>(string? actionName, string? routeName, object? routeValues, T data, string message = "Resource created successfully")
    {
        var response = new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = new List<ApiError>(),
            Timestamp = DateTime.UtcNow
        };
        return CreatedAtAction(actionName, routeName, routeValues, response);
    }

    /// <summary>
    /// Standard success response for delete
    /// </summary>
    protected IActionResult NoContent()
    {
        return base.NoContent();
    }

    /// <summary>
    /// Standard error response
    /// </summary>
    protected IActionResult Error(string message, int statusCode = 500, List<ApiError>? errors = null)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Data = null,
            Errors = errors ?? new List<ApiError> { new ApiError { Message = message } },
            Timestamp = DateTime.UtcNow
        };
        return StatusCode(statusCode, response);
    }

    /// <summary>
    /// Standard validation error response
    /// </summary>
    protected IActionResult ValidationError(string message, List<ApiError> errors)
    {
        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Data = null,
            Errors = errors,
            Timestamp = DateTime.UtcNow
        };
        return BadRequest(response);
    }

    /// <summary>
    /// Standard not found response
    /// </summary>
    protected IActionResult NotFound(string message = "Resource not found")
    {
        return Error(message, StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Standard unauthorized response
    /// </summary>
    protected IActionResult Unauthorized(string message = "Authentication required")
    {
        return Error(message, StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    /// Standard forbidden response
    /// </summary>
    protected IActionResult Forbidden(string message = "Access denied")
    {
        return Error(message, StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Standard bad request response
    /// </summary>
    protected IActionResult BadRequest(string message, List<ApiError>? errors = null)
    {
        var errorList = errors ?? new List<ApiError> { new ApiError { Message = message } };
        return ValidationError(message, errorList);
    }

    /// <summary>
    /// Get current user ID from claims
    /// </summary>
    protected int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("sub") ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }
        return null;
    }

    /// <summary>
    /// Check if user has specific role
    /// </summary>
    protected bool HasRole(string role)
    {
        return User.IsInRole(role);
    }

    /// <summary>
    /// Check if user is admin
    /// </summary>
    protected bool IsAdmin()
    {
        return HasRole("Admin");
    }

    /// <summary>
    /// Validate model state and return errors
    /// </summary>
    protected bool ValidateModel(out List<ApiError> errors)
    {
        errors = new List<ApiError>();

        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errors.Add(new ApiError { Message = error.ErrorMessage });
                }
            }
        }

        return errors.Count == 0;
    }
}
 
