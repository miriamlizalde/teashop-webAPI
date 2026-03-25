using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace TeaShop.Models

{
    public class Pedido
    {
        public enum EstadoPedido
        {
            Pendiente,
            Entregado,
            Cancelado
        }  

        public int Id { get; set; }
        public int UsuarioId { get; set; }      
        public DateTime Fecha { get; set; }
        public decimal PrecioTotal { get; set; }
        public EstadoPedido Estado { get; set; }

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

        public static void NuevoPedido (List<Usuario> usuarios, List<Producto> stock, List<Pedido> pedidos)
        {
            if (!usuarios.Any(u => !u.EsAdmin))
            {
                AnsiConsole.MarkupLine("[red]No hay usuarios registrados. Crea un usuario antes de añadir un pedido.[/]");
                Console.ReadKey();
                return;
            }

            var usuario = AnsiConsole.Prompt(
                new SelectionPrompt<Usuario>()
                    .Title("Selecciona el usuario para el pedido:")
                    .AddChoices(usuarios.Where(u => !u.EsAdmin))
                    .UseConverter(u => $"{u.Id} - {u.Nombre} ({u.Email})"));
            
            int pedidoId = pedidos.Any() ? pedidos.Max(p => p.Id) + 1 : 1;
            Pedido pe = new Pedido(pedidoId, usuario.Id);

            bool añadiendo = true;
            while (añadiendo)
            {
                var producto = AnsiConsole.Prompt(
                    new SelectionPrompt<Producto>()
                        .Title("Selecciona un producto para añadir al pedido:")
                        .AddChoices(stock)
                        .UseConverter(p => $"{p.Nombre} - (${p.Precio}/kg) - Stock: {p.Stock}g"));

                double cant = AnsiConsole.Ask<double>($"¿Cuántos gramos de {producto.Nombre} deseas añadir?");
                
                if (cant > producto.Stock)
                {
                    AnsiConsole.MarkupLine("[red]No hay suficiente stock para esa cantidad.[/]");
                    return;
                }

                pe.AñadirProducto(producto, cant);
                producto.Stock -= (int)cant;

                añadiendo = AnsiConsole.Confirm("¿Deseas añadir otro producto al pedido?");
            } 
            pedidos.Add(pe);
            AnsiConsole.MarkupLine("[green]Pedido realizado con éxito.[/]"); 
            pe.MostrarPedido();
            Console.ReadKey();      
        }
    }
    public class ItemPedido 
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public double Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}