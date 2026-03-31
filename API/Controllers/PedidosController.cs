using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeaShop.Data.Context;
using TeaShop.Models;

namespace TeaShop.API.Controllers
{   
    //URL: https://localhost:7863/api/pedidos
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly TeashopContext _context;

        public PedidosController(TeashopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos.Include(p => p.ListaProductos).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.Include(p => p.ListaProductos).FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido(Pedido nuevoPedido)
        {
            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedido), new { id = nuevoPedido.Id }, nuevoPedido);
        }    
    }
}