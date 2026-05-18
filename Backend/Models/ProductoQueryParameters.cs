using System.ComponentModel.DataAnnotations;

namespace TeaShop.Models;

public class ProductoQueryParameters
{
    [RegularExpression (@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$", ErrorMessage = "El nombre solo puede contener letras, espacios y guiones.")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
    public string? Nombre { get; set; }

    [RegularExpression (@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$", ErrorMessage = "El origen solo puede contener letras, espacios y guiones.")]
    [StringLength(100, ErrorMessage = "El origen no puede tener más de 100 caracteres.")]
    public string? Origen { get; set; }

    public string? Tipo { get; set; }
    public bool? EsOrganico { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio mínimo debe ser mayor que cero.")]
    public decimal? PrecioMin { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio máximo debe ser mayor que cero.")]
    public decimal? PrecioMax { get; set; }

    public string OrdenarPor { get; set; } = "Nombre";
    public bool Descendente { get; set; } = false;
    
}