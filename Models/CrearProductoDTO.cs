using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TeaShop.Models;
public class CrearProductoDTO
{
    [Required]
    public string Tipo { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Origen { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public bool EsOrganico { get; set; }
    [Required]
    public DateTime? FechaCaducidad { get; set; }
    public string? TipoHoja { get; set; }
    public string? TipoComida { get; set; }
    public bool? Gluten { get; set; }
    public IFormFile? Imagen { get; set; }
}