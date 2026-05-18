using Microsoft.EntityFrameworkCore;
using TeaShop.Models;

namespace TeaShop.Data
{
    public class PedidoEFRepository : IPedidoRepository
    {
        private readonly TeashopContext _context;

        public PedidoEFRepository(TeashopContext context)
        {
            _context = context;
        }

        public void AddPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public IEnumerable<Pedido> GetAllPedidos(PedidoQueryParameters? queryParameters = null, int? usuarioId = null)
        {
            var query = _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Items).ThenInclude(i => i.Producto)
            .AsQueryable();

            if (usuarioId.HasValue)
                query = query.Where(p => p.UsuarioId == usuarioId.Value);

            if (!string.IsNullOrWhiteSpace(queryParameters?.Estado))
                query = query.Where(p => p.Estado.Contains(queryParameters.Estado));

            if (queryParameters?.FechaDesde.HasValue == true)
                query = query.Where(p => p.Fecha >= queryParameters.FechaDesde.Value);

            if (queryParameters?.FechaHasta.HasValue == true)
                query = query.Where(p => p.Fecha <= queryParameters.FechaHasta.Value);

            if (queryParameters?.OrdenarPor?.ToLower() == "fecha")
                query = queryParameters.Descendente ? query.OrderByDescending(p => p.Fecha) : query.OrderBy(p => p.Fecha);
            else if (queryParameters?.OrdenarPor?.ToLower() == "total")
                query = queryParameters.Descendente ? query.OrderByDescending(p => p.PrecioTotal) : query.OrderBy(p => p.PrecioTotal);
            else
                query = queryParameters?.Descendente == true ? query.OrderByDescending(p => p.PedidoId) : query.OrderBy(p => p.PedidoId);            

            return query.ToList();
        }

        public Pedido GetPedido(int pedidoId, int? usuarioId)
        {
            var query = _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Items).ThenInclude(i => i.Producto)
            .Where(p => p.PedidoId == pedidoId);

            if (usuarioId.HasValue)
                query = query.Where(p => p.UsuarioId == usuarioId.Value);

            return query.FirstOrDefault();
        }

        public void UpdateEstado(int pedidoId, string nuevoEstado)
        {
            var pedido = _context.Pedidos.Find(pedidoId);
            if (pedido == null) throw new KeyNotFoundException($"Pedido con ID {pedidoId} no encontrado.");

            pedido.Estado = nuevoEstado;
            SaveChanges();
        }
        public void DeletePedido(int pedidoId)
        {
            var pedido = _context.Pedidos.Find(pedidoId);
            if (pedido == null) throw new KeyNotFoundException($"Pedido con ID {pedidoId} no encontrado.");

            _context.Pedidos.Remove(pedido);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

    }  
}         