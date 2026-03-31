using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaShop.Models;

public class Pedido
{
    [Key]
    public int PedidoId { get; set; }

    [ForeignKey("Usuario")]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }    
    public DateTime Fecha { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecioTotal { get; set; }
    public string Estado { get; set; }

    public List<ItemPedido> Items { get; set; } = new List<ItemPedido>();
}

public class ItemPedido 
{
    [Key]
    public int ItemPedidoId { get; set; }

    [ForeignKey("Pedido")]
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; }

    [ForeignKey("Producto")]
    public int ProductoId { get; set; } 
    public Producto Producto { get; set; }
    public double Cantidad { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal Precio { get; set; }
}