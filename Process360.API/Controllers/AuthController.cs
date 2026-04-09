using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Process360.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Generates a JWT token for testing purposes
        /// </summary>
        /// <param name="username">Username for the token (default: testuser)</param>
        /// <returns>JWT Bearer token</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromQuery] string username = "testuser")
        {
            try
            {
                var jwtSettings = _configuration.GetSection("Jwt");
                var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];
                var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, username),
                    new Claim("iss", issuer),
                    new Claim("aud", audience)
                };

                var key = new SymmetricSecurityKey(secretKey);
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation("JWT token generated for user: {Username}", username);

                return Ok(new
                {
                    token = tokenString,
                    expiresIn = expirationMinutes * 60,
                    tokenType = "Bearer",
                    username = username
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token");
                return BadRequest(new { message = "Error generating token", error = ex.Message });
            }
        }

        /// <summary>
        /// Test endpoint to verify JWT authentication is working
        /// </summary>
        /// <returns>Success message with user information</returns>
        [HttpGet("test")]
        [Authorize]
        public IActionResult TestAuth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                message = "Authentication successful!",
                userId = userId,
                userName = userName,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
