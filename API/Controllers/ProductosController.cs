using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeaShop.Business;
using TeaShop.Models;

namespace TeaShop.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ILogger<ProductosController> _logger;
        private readonly IProductoService _productoService;
        private readonly IAuthService _authService;
        private readonly IImageService _imageService;

        public ProductosController(ILogger<ProductosController> logger, IProductoService productoService, IImageService imageService)
        {
            _logger          = logger;
            _productoService = productoService;
            _imageService    = imageService;
        }

        // GET: /Productos
        [HttpGet(Name = "GetAllProductos")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Producto>> GetProductos(
            [FromQuery] ProductoQueryParameters queryParameters)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                var productos = _productoService.GetAllProductos(queryParameters);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: /Productos/5
        [HttpGet("{productoId}", Name = "GetProducto")]
        [AllowAnonymous]
        public IActionResult GetProducto(int productoId)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(ModelState); }

                var producto = _productoService.GetProducto(productoId);
                return Ok(producto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No encontrado el producto " + productoId);
            }
        }

        // POST: /Productos
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task <IActionResult> AddProducto([FromForm] CrearProductoDTO productoDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                await _productoService.AddProducto(productoDTO);
                return Ok("Producto creado correctamente.");
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // PUT: /Productos/5
        [HttpPut("{productoId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateProducto(int productoId, [FromForm] CrearProductoDTO productoDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                await _productoService.UpdateProducto(productoId, productoDTO);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: /Productos/5
        [HttpDelete("{productoId}")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult DeleteProducto(int productoId)
        {
            try
            {
                _productoService.DeleteProducto(productoId);
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