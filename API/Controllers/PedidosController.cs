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
    public class PedidosController : ControllerBase
    {
        private readonly ILogger<PedidosController> _logger;
        private readonly IPedidoService _pedidoService;
        private readonly IAuthService   _authService;

        public PedidosController(ILogger<PedidosController> logger,
                                 IPedidoService pedidoService,
                                 IAuthService authService)
        {
            _logger         = logger;
            _pedidoService  = pedidoService;
            _authService    = authService;
        }

        [HttpGet(Name = "GetAllPedidos")]
        public ActionResult<IEnumerable<Pedido>> GetPedidos(
            [FromQuery] PedidoQueryParameters queryParameters)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                int? usuarioId = null;
                if (!User.IsInRole(Roles.Admin))
                {
                    var claim = User.Claims.FirstOrDefault(
                        c => c.Type == ClaimTypes.NameIdentifier);
                    if (claim != null) usuarioId = int.Parse(claim.Value);
                }

                var pedidos = _pedidoService.GetAllPedidos(queryParameters, usuarioId);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{pedidoId}", Name = "GetPedido")]
        public IActionResult GetPedido(int pedidoId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                int? usuarioId = null;
                if (!User.IsInRole(Roles.Admin))
                {
                    var claim = User.Claims.FirstOrDefault(
                        c => c.Type == ClaimTypes.NameIdentifier);
                    if (claim != null) usuarioId = int.Parse(claim.Value);
                }

                var pedido = _pedidoService.GetPedido(pedidoId, usuarioId);
                return Ok(pedido);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Pedido no encontrado.");
            }
        }

        [HttpPost]
        public IActionResult AddPedido([FromBody] CrearPedidoDTO pedidoDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var claim = User.Claims.FirstOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier);
                if (claim == null) return Unauthorized();
                int usuarioId = int.Parse(claim.Value);

                _pedidoService.AddPedido(usuarioId, pedidoDTO);
                return Ok("Pedido creado correctamente.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{pedidoId}/estado")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult UpdateEstado(int pedidoId, [FromBody] string nuevoEstado)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                _pedidoService.UpdateEstado(pedidoId, nuevoEstado);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Pedido no encontrado.");
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

        [HttpDelete("{pedidoId}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeletePedido(int pedidoId)
        {
            try
            {
                _pedidoService.DeletePedido(pedidoId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}