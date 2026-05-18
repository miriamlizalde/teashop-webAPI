using System.ComponentModel.DataAnnotations;

namespace TeaShop.Models;

public class CrearPedidoDTO
{
    [Required]
    public int UsuarioId { get; set; }
    [Required]
    public DateTime Fecha { get; set; }
    [Required]
    public List<ItemCrearPedidoDTO> Items { get; set; } 
    [Required]
    public decimal PrecioTotal { get; set; }

}

public class ItemCrearPedidoDTO
{
    [Required]
    public int ProductoId { get; set; }
    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0.")]
    public double Cantidad { get; set; }
}