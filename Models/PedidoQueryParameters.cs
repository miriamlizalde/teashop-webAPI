using System.ComponentModel.DataAnnotations;

namespace TeaShop.Models;

public class PedidoQueryParameters
{
    public int? UsuarioId { get; set; }
    
    [RegularExpression(@"^(Pendiente|Entregado|Cancelado)$", ErrorMessage = "El estado del pedido debe ser Pendiente, Entregado o Cancelado.")]
    public string? Estado { get; set; }

    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio mínimo debe ser un número positivo.")]
    public decimal? PrecioMin { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio máximo debe ser un número positivo.")]
    public decimal? PrecioMax { get; set; }

    public string OrdenarPor { get; set; } = "Fecha"; 
    public bool Descendente { get; set; } = true;
}