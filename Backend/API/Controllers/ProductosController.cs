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
                var result = productos.Select(p => new ProductoDTO
                {
                    ProductoId = p.ProductoId,
                    Nombre = p.Nombre,
                    Origen = p.Origen,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    EsOrganico = p.EsOrganico,
                    FechaCaducidad = p.FechaCaducidad,
                    TipoProducto = p is Te ? "Te" : "Comida",
                    TipoHoja = p is Te te ? te.TipoHoja : null,
                    TipoComida = p is Comida c ? c.TipoComida : null,
                    Gluten = p is Comida comida ? comida.Gluten : null,
                    ImagenUrl = p.ImagenUrl
                });
                return Ok(result);
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
                var result = new ProductoDTO
                {
                    ProductoId = producto.ProductoId,
                    Nombre = producto.Nombre,
                    Origen = producto.Origen,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    EsOrganico = producto.EsOrganico,
                    FechaCaducidad = producto.FechaCaducidad,
                    TipoProducto = producto is Te ? "Te" : "Comida",
                    TipoHoja = producto is Te te ? te.TipoHoja : null,
                    TipoComida = producto is Comida c ? c.TipoComida : null,
                    Gluten = producto is Comida comida ? comida.Gluten : null,
                    ImagenUrl = producto.ImagenUrl
                };
                return Ok(result);
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