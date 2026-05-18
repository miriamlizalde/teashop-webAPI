using TeaShop.Models;
using TeaShop.Data;

namespace TeaShop.Business;
public class PedidoService : IPedidoService
{

    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProductoRepository _productoRepository;
    public PedidoService(IPedidoRepository pedidoRepository, IProductoRepository productoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _productoRepository = productoRepository;
    }

    public IEnumerable<Pedido> GetAllPedidos(PedidoQueryParameters? queryParameters, int? usuarioId)
    {
        return _pedidoRepository.GetAllPedidos(queryParameters, usuarioId);
    }

    public Pedido GetPedido(int pedidoId, int? usuarioId)
        {
        var pedido = _pedidoRepository.GetPedido(pedidoId, usuarioId);
        if (pedido == null)
            throw new KeyNotFoundException("Pedido no encontrado.");
        return pedido;
    }

    public Pedido AddPedido(int usuarioId, CrearPedidoDTO dto)
    {
        if (dto.Items == null || !dto.Items.Any())
            throw new ArgumentException("El pedido debe contener al menos un producto.");
    
        var pedido = new Pedido
        {
        UsuarioId = usuarioId,
        Fecha = DateTime.Now,
        Estado = "Pendiente",
        PrecioTotal = 0
        };

    foreach (var item in dto.Items)
    {
        var producto = _productoRepository.GetProducto(item.ProductoId);
        if (producto == null)
            throw new KeyNotFoundException($"Producto con ID {item.ProductoId} no encontrado.");
        if (producto.Stock < item.Cantidad)
            throw new InvalidOperationException($"No hay suficiente stock para el producto {producto.Nombre}.");

        decimal subtotal = producto.Precio * (decimal)(item.Cantidad/1000.0);

        pedido.Items.Add(new ItemPedido
        {
            ProductoId = item.ProductoId,
            Cantidad = item.Cantidad,
            Precio = producto.Precio
        });

        producto.Stock -= (int)item.Cantidad;
        _productoRepository.UpdateProducto(producto);
        pedido.PrecioTotal += subtotal;
    }

    _pedidoRepository.AddPedido(pedido);
    _pedidoRepository.SaveChanges();
    return pedido;
}

public void UpdateEstado(int pedidoId, string nuevoEstado)
{
    var estadosOk = new[] { "Pendiente", "Enviado", "Entregado", "Cancelado" };
    if (!estadosOk.Contains(nuevoEstado))
        throw new ArgumentException("Estado no válido. Debe ser 'Pendiente', 'Enviado', 'Entregado' o 'Cancelado'.");
    var pedido = _pedidoRepository.GetPedido(pedidoId, null);
    if (pedido == null)
        throw new KeyNotFoundException("Pedido no encontrado.");
    if (pedido.Estado != "Pendiente")
        throw new InvalidOperationException("Solo se pueden actualizar pedidos pendientes.");
    if (pedido.Estado == "Entregado" || pedido.Estado == "Cancelado")
        throw new InvalidOperationException("No se pueden modificar pedidos entregados o cancelados.");

    _pedidoRepository.UpdateEstado(pedidoId, nuevoEstado);    
}

public void DeletePedido(int pedidoId)
{
    var pedido = _pedidoRepository.GetPedido(pedidoId, null);
    if (pedido == null)
        throw new KeyNotFoundException("Pedido no encontrado.");
    if (pedido.Estado != "Pendiente")
        throw new InvalidOperationException("Solo se pueden eliminar pedidos pendientes.");

    _pedidoRepository.DeletePedido(pedidoId);
}
}