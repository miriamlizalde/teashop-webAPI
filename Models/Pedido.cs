using System;
using System.Collections.Generic;

namespace TeaShop.Models

{
    public class Pedido
    {
        public enum EstadoPedido
        {
            Pendiente,
            Enviado,
            Entregado,
            Cancelado
        }  

        public int Id { get; set; }
        public int UsuarioId { get; set; }      
        public DateTime Fecha { get; set; }
        public decimal PrecioTotal { get; set; }
        public EstadoPedido Estado { get; set; }
        public bool EsRegalo { get; set; }

        public List<ItemPedido> ListaProductos { get; set; } = new List<ItemPedido>();

        public Pedido(int id, int usuarioId)
        {
            Id = id;
            UsuarioId = usuarioId;
            PrecioTotal = 0;
            Estado = EstadoPedido.Pendiente;
            Fecha = DateTime.Now;
        }
        
        public void AñadirProducto(Producto p, double cantidad)
        {
            ListaProductos.Add(new ItemPedido 
            {
                ProductoId = p.Id,
                Nombre = p.Nombre,
                Cantidad = cantidad,
                Precio = p.Precio
            });
            PrecioTotal += p.Precio * (decimal)(cantidad / 1000.0); 
        }

        public void MostrarPedido()
        {
            string FormatoFecha = "dd/MM/yyyy HH:mm";

            Console.WriteLine($"\n---- Detalles del Pedido #{Id} ----");
            Console.WriteLine($"Usuario ID: {UsuarioId} | fecha: {Fecha.ToString(FormatoFecha)}");
            Console.WriteLine($"Estado del Pedido: {Estado}");
            Console.WriteLine("---------------------------------------------");
            foreach (var item in ListaProductos)
            {
                decimal subtotal = item.Precio * (decimal)(item.Cantidad / 1000.0);
                Console.WriteLine($"- {item.Nombre} | {item.Cantidad}g | {item.Precio:C}/kg | Subtotal: {subtotal:C}");
            }
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"TOTAL A PAGAR : ${PrecioTotal:F2}");
            
            DateTime fechaEntrega = Fecha.AddMinutes(30);
            Console.WriteLine($"Fecha Estimada de Entrega: {fechaEntrega.ToString(FormatoFecha)}");
        } 
    }
    public class ItemPedido 
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public double Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}