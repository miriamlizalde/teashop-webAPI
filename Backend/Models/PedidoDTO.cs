namespace TeaShop.Models
{
    public class PedidoDTO
    {
        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<ItemPedidoDTO> Items { get; set; } = new();
    }

    public class ItemPedidoDTO
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public double Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}