using System.ComponentModel.DataAnnotations;

namespace TeaShop.Models;

public class PedidoQueryParameters
{
    [RegularExpression(@"^(Pendiente|Entregado|Cancelado)$", ErrorMessage = "El estado del pedido debe ser Pendiente, Entregado o Cancelado.")]
    public string? Estado { get; set; }

    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio mínimo debe ser un número positivo.")]
    public decimal? PrecioMinimo { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El precio máximo debe ser un número positivo.")]
    public decimal? PrecioMaximo { get; set; }

    public string OrdenarPor { get; set; } = "Fecha"; 
}