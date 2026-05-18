using Microsoft.AspNetCore.Mvc;
using TeaShop.Business;
using TeaShop.Models;

namespace TeaShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: /Auth/Login
        [HttpPost("Login")]
        public IActionResult Login(LoginDtoIn loginDtoIn)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                var token = _authService.Login(loginDtoIn);
                return Ok(token);
            }
            catch (KeyNotFoundException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error generando el token: " + ex.Message);
            }
        }

        // POST: /Auth/Register
        [HttpPost("Register")]
        public IActionResult Register(UsuarioDtoIn usuarioDtoIn)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                var token = _authService.Register(usuarioDtoIn);
                return Ok(token);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error generando el token: " + ex.Message);
            }
        }
    }
}