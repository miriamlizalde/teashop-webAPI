namespace TeaShop.Models
{
    public class ProductoDTO
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Origen { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool EsOrganico { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public string TipoProducto { get; set; } = string.Empty;
        public string? TipoHoja { get; set; }
        public string? TipoComida { get; set; }
        public bool? Gluten { get; set; }
        public string? ImagenUrl { get; set; }
    }
}