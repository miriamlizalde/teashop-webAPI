using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaShop.Models;

public abstract class Producto
    {
        [Key]
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Origen { get; set; } 
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool EsOrganico { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public string? ImagenUrl { get; set; }

       public List<ItemPedido> ItemPedido { get; set; } = new List<ItemPedido>();
    }

    public class Te : Producto
    {
        public string TipoHoja { get; set; } 
    }    

    public class Comida : Producto
    {
        public string TipoComida { get; set; } 
        public bool Gluten {get; set; }
    }
