using System;
using System.Collections.Generic;
using System.Linq;
using TeaShop.Models;
using TeaShop.Utils;
using Spectre.Console; 

class Program
{
    static List<Producto> stock = new List<Producto>();
    static List<Usuario> usuarios = new List<Usuario>();
    static List<Pedido> pedidos = new List<Pedido>();
    static Usuario? usuarioIdentificado = null;

    static void Main(string[] args)
    {
        CargarDatosIniciales();

        if (!usuarios.Any()) {
            usuarios.Add(new Usuario(1, "admin", "admin@gmail.com", "admin", false) { EsAdmin = true });
            TeashopManager.GuardarDatos(usuarios, "usuarios.json");
        }
        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("TeaShop").Centered().Color(Color.Green));

            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Selecciona una opción:[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Ver Catálogo", 
                        "Buscar Producto",
                        "Iniciar sesión", 
                        "Mi perfil", 
                        "Gestión TeaShop - Admin",
                        "Salir"
                    }));

            switch (opcion)
            {
                case "Ver Catálogo":
                    MostrarCatalogo();
                    break;
                case "Buscar Producto":
                    BuscarProducto();
                    break;
                case "Iniciar sesión":
                    Login();
                    break;
                case "Mi perfil":
                    ZonaPrivada();
                    break;
                case "Gestión TeaShop - Admin":
                    Admin();
                    break;
                case "Salir":
                    salir = true;
                    break;
            }
        }
    }

    // ZONA PÚBLICA
    static void MostrarCatalogo()
    {
        var tabla = new Table().Border(TableBorder.Rounded).Title("[bold blue]-- CATÁLOGO DE PRODUCTOS --[/]");
        tabla.AddColumn("ID");
        tabla.AddColumn("Nombre");
        tabla.AddColumn("Origen");
        tabla.AddColumn("Precio/kg");
        tabla.AddColumn("Tipo");
        tabla.AddColumn("Orgánico");
        tabla.AddColumn("Gluten");
        tabla.AddColumn("Stock(g)");
        tabla.AddColumn("Caducidad");

        foreach (var p in stock)
        {
            string tipo = p is Te ? "Té" : "Comida";
            tabla.AddRow(
                p.Id.ToString(), 
                p.Nombre,
                p.Origen, 
                $"{p.Precio:F2}€", 
                tipo, 
                p.EsOrganico ? "[green]Sí[/]" : "[red]No[/]",
                p is Comida comida ? (comida.ContieneGluten ? "[green]Sí[/]" : "[red]No[/]") : "N/A",
                p.Stock.ToString(), 
                p.FechaCaducidad.ToShortDateString());
        }
        AnsiConsole.Write(tabla);
        AnsiConsole.MarkupLine("[white]Presiona cualquier tecla para continuar[/]");
        Console.ReadKey();
    }

    // BÚSQUEDA
    static void BuscarProducto()
    {
        string busqueda = AnsiConsole.Ask<string>("Introduce el nombre del producto que buscas:");
        var resultados = stock.Where(p => p.Nombre.Contains(busqueda, StringComparison.OrdinalIgnoreCase)).ToList();
        
        if (resultados.Any())
        {
            foreach (var r in resultados) r.MostrarDetalles();
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No se encontraron productos.[/]");
        }
        Console.ReadKey();
    }

    // GESTIÓN ADMIN
    static void Admin()
    {
        if (usuarioIdentificado == null || !usuarioIdentificado.EsAdmin)
        {
            AnsiConsole.MarkupLine("[red]Acceso denegado. Se requiere cuenta de Administrador.[/]");
            Console.ReadKey();
            return;
        }

        var subOpcion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Gestión de TeaShop[/]")
                .AddChoices(new[] { 
                    "Dar de alta Té", 
                    "Dar de alta Comida",
                    "Dar de alta Usuario",
                    "Añadir Pedido a Usuario",
                    "Añadir Saldo a Usuario",
                    "Ver Usuarios", 
                    "Volver" 
                    }));

        switch (subOpcion)
        {
            case "Dar de alta Té":
                Te.NuevoTe(stock);
                TeashopManager.GuardarDatos(stock, "productos.json");
                break;
            case "Dar de alta Comida":
                Comida.NuevaComida(stock);
                TeashopManager.GuardarDatos(stock, "productos.json");
                break;
            case "Dar de alta Usuario":
                Usuario.NuevoUsuario(usuarios);
                TeashopManager.GuardarDatos(usuarios, "usuarios.json");
                break;
            case "Añadir Pedido a Usuario":
                Pedido.NuevoPedido(usuarios, stock, pedidos);
                TeashopManager.GuardarDatos(pedidos, "pedidos.json");
                TeashopManager.GuardarDatos(usuarios, "usuarios.json");
                break;
            case "Añadir Saldo a Usuario":
                Usuario.AñadirSaldo(usuarios);
                TeashopManager.GuardarDatos(usuarios, "usuarios.json");
                break;    
            case "Ver Usuarios":
                ListaUsuarios();
                break;
            case "Volver":
                break;
        }
    }

    // ZONA PRIVADA
    static void ZonaPrivada()
    {
        if (usuarioIdentificado == null)
        {
            AnsiConsole.MarkupLine("[red]Debes iniciar sesión para ver tu perfil.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[bold blue]BIENVENIDO A TU PERFIL, {usuarioIdentificado.Nombre}[/]");
            usuarioIdentificado.MostrarDetalles();
            AnsiConsole.MarkupLine("\n[blue]-- TUS PEDIDOS --[/]");
            var misPedidos = pedidos.Where(p => p.UsuarioId == usuarioIdentificado.Id).ToList();

            if (misPedidos.Any())
            {
                foreach (var pedido in misPedidos)
                {
                    pedido.MostrarPedido();
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]No tienes pedidos realizados.[/]");
            }
    
        }
        Console.ReadKey();
    }

    static void Login()
    {
        AnsiConsole.MarkupLine("[bold blue]-- IDENTIFICACIÓN --[/]");
        string nombre = AnsiConsole.Ask<string>("Nombre de usuario:");
        var existeUsuario = usuarios.FirstOrDefault(u => u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        
        if (existeUsuario == null)
        {
            AnsiConsole.MarkupLine("[red]El usuario no existe.[/]");
   
        }
        else
        {
            string pass = AnsiConsole.Prompt(
                new TextPrompt<string>("Contraseña:")
                    .PromptStyle("red")
                    .Secret());

            if (existeUsuario.Password == pass)
            {
                usuarioIdentificado = existeUsuario;
                AnsiConsole.MarkupLine($"[green]¡Bienvenido, {usuarioIdentificado.Nombre}![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Contraseña incorrecta.[/]");
                Console.ReadKey();
                return;
            }        
            
        }
        Console.ReadKey();
    }

    static void CargarDatosIniciales()
    {
        try 
        {
            stock = TeashopManager.CargarDatos<Producto>("productos.json");
            usuarios = TeashopManager.CargarDatos<Usuario>("usuarios.json");
            pedidos = TeashopManager.CargarDatos<Pedido>("pedidos.json");

            if (!stock.Any())
            {
                stock.Add(new Te(1, "Té Matcha", "Japón", 50.5m, 1000, true, "Verde"));
                stock.Add(new Te(2, "Té Chai", "India", 32.00m, 10000, false, "Negro"));
                stock.Add(new Te(3, "Té Puerh", "China", 22.00m, 10000, false, "Rojo"));
                stock.Add(new Te(4, "Té Oolong", "China", 45.00m, 1500, true, "Azul"));

                stock.Add(new Comida(5, "Tarta de Pistacho", "España", 25.0m, 5000, false, "Dulce", true));
                stock.Add(new Comida(6, "Cookie de Avena", "España", 17.80m, 2500, true, "Dulce", false));
                stock.Add(new Comida(7, "Sándwich Vegetal", "Francia", 23.50m, 2200, false, "Salado", true));
                stock.Add(new Comida(8, "Wrap de Pollo", "Reino Unido", 20.20m, 6400, true, "Salado", false));
                
                TeashopManager.GuardarDatos(stock, "productos.json");
            }

            if (!usuarios.Any()) {
                var admin = new Usuario (1, "admin", "admin@gmail.com", "admin", true);
                admin.EsAdmin = true;
                usuarios.Add(admin);
                TeashopManager.GuardarDatos(usuarios, "usuarios.json");
            }

        }
        catch (Exception ex)
        {
            TeashopManager.GuardarLog(ex.Message);
        }
    }    
    
    static void ListaUsuarios()
    {
        var tabla = new Table().Border(TableBorder.Rounded).Title("[bold blue]-- CLIENTES REGISTRADOS --[/]");
        tabla.AddColumn("ID");
        tabla.AddColumn("Nombre");
        tabla.AddColumn("Email");
        tabla.AddColumn("Saldo");
        tabla.AddColumn("Es Admin");
        tabla.AddColumn("Socio");

        foreach (var u in usuarios){
            string esAdmin = u.EsAdmin ? "[yellow]SÍ[/]" : "No";
            string esSocio = u.EsSocio ? "[blue]SÍ[/]" : "No";
        
            tabla.AddRow(u.Id.ToString(), u.Nombre, u.Email, u.Saldo.ToString("F2")+" Euros", esAdmin, esSocio);
        }
        AnsiConsole.Write(tabla);
        Console.ReadKey();
    }
       
}