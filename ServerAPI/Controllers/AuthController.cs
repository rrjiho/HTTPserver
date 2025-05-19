using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Models;
using ServerAPI.Services;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            try
            {
                await _userService.RegisterAsync(dto);
                return Ok("User registered successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var token = await _userService.LoginAsync(dto);
                return Ok(new {Token = token});
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return Unauthorized(e.Message);
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Log in is required.");
            }

            try
            {
                var user = await _userService.GetProfileAsync(userId);
                var resources = await _userService.GetResourcesAsync(userId);
                return Ok(new { user.Username, user.Level, user.Experience, user.Strength, 
                    user.Agility, user.Intelligence, resources.Gold, resources.Gems });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return StatusCode(500, e.Message);
            }
            
        }

        [Authorize]
        [HttpPost("addexp")]
        public async Task<IActionResult> AddExperience([FromBody] AddExpDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Log in is required.");
            }

            try
            {
                await _userService.AddExperienceAsync(userId, dto.Experience);
                var user = await _userService.GetProfileAsync(userId);
                return Ok(new { LevelUp = user.Experience >= 100, user.Level, user.Experience });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return StatusCode(500, e.Message);
            }
            
        }

        [Authorize]
        [HttpGet("resources")]
        public async Task<IActionResult> GetResources()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Log in is required.");
            }

            try
            {
                var resources = await _userService.GetResourcesAsync(userId);
                return Ok(new { resources.Gold, resources.Gems });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return NotFound(e.Message);
            }
        }

        [Authorize]
        [HttpPost("addresources")]
        public async Task<IActionResult> AddResources([FromBody] ResourcesDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Log in is required.");
            }

            try
            {
                await _userService.AddResourcesAsync(userId, dto.Gold, dto.Gems);
                var resources = await _userService.GetResourcesAsync(userId);
                return Ok(new { resources.Gold, resources.Gems });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return NotFound(e.Message);
            }

        }

        [Authorize]
        [HttpPost("useresource")]
        public async Task<IActionResult> UseResources([FromBody] ResourcesDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Log in is required.");
            }

            try
            {
                await _userService.UseResourcesAsync(userId, dto.Gold, dto.Gems);
                var resources = await _userService.GetResourcesAsync(userId);
                return Ok(new { resources.Gold, resources.Gems });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}
