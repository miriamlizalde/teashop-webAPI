using TeaShop .Models;

namespace TeaShop.Data
{
    public interface IPedidoRepository
    {
        void AddPedido(Pedido pedido);
        IEnumerable<Pedido> GetAllPedidos(PedidoQueryParameters? queryParameters = null, 
            int? usuarioId = null);
        Pedido GetPedido(int pedidoId, int? usuarioId = null);
        void UpdateEstado(int pedidoId, string nuevoEstado);
        void DeletePedido(int pedidoId);
        void SaveChanges();
    }
}