using TeaShop.Models;

namespace TeaShop.Business
{
    public interface IPedidoService
    {
        IEnumerable<Pedido> GetAllPedidos(PedidoQueryParameters? queryParameters = null, int? usuarioId = null);
        Pedido GetPedido(int pedidoId, int? usuarioId = null);
        Pedido AddPedido(int usuarioId, CrearPedidoDTO pedidoDTO);
        void UpdateEstado(int pedidoId, string nuevoEstado);
        void DeletePedido(int pedidoId);
    }
}