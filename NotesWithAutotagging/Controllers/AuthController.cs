using Microsoft.AspNetCore.Mvc;
using NotesWithAutotagging.Models;
using NWA.Application.DTOs;
using NWA.Application.Interfaces;
using NWA.Application.Services;

namespace NotesWithAutotagging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtGeneratorService _jwtGeneratorService;
        private readonly IUserService _userService;

        public AuthController(IJwtGeneratorService jwtGeneratorService, IUserService userService)
        {
            _jwtGeneratorService = jwtGeneratorService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var newUser = await _userService.RegisterAsync(model.Username, model.Password, model.Email);

            if (newUser == null)
            {
                return BadRequest("Nie można utworzyć użytkownika.");
            }

            
            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var userIsValid = await _userService.CheckPasswordAsync(model.Username, model.Password);

                if (!userIsValid)
                {
                    return Unauthorized("Wrong username or password.");
                }

                var user = await _userService.GetUserByUsernameAsync(model.Username);
                var token = _jwtGeneratorService.GenerateToken(user);

                return Ok(new { Token = token });
            }
            catch(ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, "InternalServerError");
            }

        }
    }
}
