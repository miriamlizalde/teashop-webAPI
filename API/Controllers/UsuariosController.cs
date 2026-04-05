using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeaShop.Business;
using TeaShop.Models;

namespace TeaShop.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ILogger<UsuariosController> _logger;
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(ILogger<UsuariosController> logger,
                                  IUsuarioService usuarioService)
        {
            _logger         = logger;
            _usuarioService = usuarioService;
        }

        // GET: /Usuarios/me 
        [HttpGet("me", Name = "GetMiPerfil")]
        public IActionResult GetMiPerfil()
        {
            try
            {
                var claim = User.Claims.FirstOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier);
                if (claim == null) return Unauthorized();
                int usuarioId = int.Parse(claim.Value);

                var usuario = _usuarioService.GetUsuario(usuarioId);
                return Ok(usuario);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Usuario no encontrado.");
            }
        }

        // GET: /Usuarios (solo Admin)
        [HttpGet(Name = "GetAllUsuarios")]
        [Authorize(Roles = Roles.Admin)]
        public ActionResult<IEnumerable<UsuarioDtoOut>> GetUsuarios()
        {
            try
            {
                var usuarios = _usuarioService.GetAllUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{usuarioId}", Name = "GetUsuario")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetUsuario(int usuarioId)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }
                var usuario = _usuarioService.GetUsuario(usuarioId);
                return Ok(usuario);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No encontrado el usuario " + usuarioId);
            }
        }

        // POST: /Usuarios/5/saldo 
        [HttpPost("{usuarioId}/saldo")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult AñadirSaldo(int usuarioId, [FromBody] decimal cantidad)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }
                _usuarioService.AñadirSaldo(usuarioId, cantidad);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No encontrado el usuario " + usuarioId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: /Usuarios/5 
        [HttpDelete("{usuarioId}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeleteUsuario(int usuarioId)
        {
            try
            {
                _usuarioService.DeleteUsuario(usuarioId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound();
            }
        }
     
    }
}