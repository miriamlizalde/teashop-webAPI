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
            AnsiConsole.Write(new FigletText("TeaShop").Centered().Color(Color.LightGreen));

            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Selecciona una opción:[/]")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Ver Catálogo (Zona Pública)", 
                        "Buscar Producto",
                        "Identificarse / Registrarse", 
                        "Zona Privada (Mis Pedidos)", 
                        "Gestión Admin (Altas)",
                        "Salir"
                    }));

            switch (opcion)
            {
                case "Ver Catálogo (Zona Pública)":
                    MostrarCatalogo();
                    break;
                case "Buscar Producto":
                    BuscarProducto();
                    break;
                case "Identificarse / Registrarse":
                    Login();
                    break;
                case "Zona Privada (Mis Pedidos)":
                    ZonaPrivada();
                    break;
                case "Gestión Admin (Altas)":
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
        var tabla = new Table().Border(TableBorder.Rounded);
        tabla.AddColumn("ID");
        tabla.AddColumn("Nombre");
        tabla.AddColumn("Precio/kg");
        tabla.AddColumn("Tipo");

        foreach (var p in stock)
        {
            string tipo = p is Te ? "Té" : "Comida";
            tabla.AddRow(p.Id.ToString(), p.Nombre, $"{p.Precio:F2}€", tipo);
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
                .Title("Gestión de Almacén")
                .AddChoices(new[] { "Dar de alta Té", "Dar de alta Comida", "Volver" }));

        if (subOpcion == "Volver") return;

        // Lógica de Alta
        int id = stock.Count + 1;
        string nombre = AnsiConsole.Ask<string>("Nombre del producto:");
        decimal precio = AnsiConsole.Ask<decimal>("Precio:");

        if (subOpcion == "Dar de alta Té")
        {
            string tipoHoja = AnsiConsole.Ask<string>("Tipo de hoja:");
            stock.Add(new Te(id, nombre, "Importación", precio, 1000, true, tipoHoja, null));
        }
        else
        {
         stock.Add(new Comida(id, nombre, "Local", precio, 50, false, "Reposteria", false, null));
        }

        TeashopManager.GuardarDatos (stock, "productos.json");
        AnsiConsole.MarkupLine("[green]Producto guardado con éxito y persistido en JSON.[/]");
        Console.ReadKey();
    }

    // ZONA PRIVADA
    static void ZonaPrivada()
    {
        if (usuarioIdentificado == null)
        {
            AnsiConsole.MarkupLine("[red]Debes iniciar sesión para ver tu zona privada.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[blue]Bienvenido a tu perfil, {usuarioIdentificado.Nombre}[/]");
            usuarioIdentificado.MostrarDetalles();
            // Aquí se listarían los pedidos filtrados por usuarioIdentificado.Id
        }
        Console.ReadKey();
    }

    static void Login()
    {
        string nombre = AnsiConsole.Ask<string>("Nombre de usuario:");
        // Simulación rápida de registro/login
        usuarioIdentificado = usuarios.FirstOrDefault(u => u.Nombre == nombre);
        
        if (usuarioIdentificado == null)
        {
            usuarioIdentificado = new Usuario(usuarios.Count + 1, nombre, $"{nombre}@gmail.com", "1234", false);
            usuarios.Add(usuarioIdentificado);
            TeashopManager.GuardarDatos(usuarios, "usuarios.json");
            AnsiConsole.MarkupLine("[green]Nuevo usuario registrado.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[green]¡Bienvenido, {usuarioIdentificado.Nombre}![/]");
        }
        Console.ReadKey();
    }

    static void CargarDatosIniciales()
    {
        try 
        {
            stock = TeashopManager.CargarDatos<Producto>("productos.json");
            usuarios = TeashopManager.CargarDatos<Usuario>("usuarios.json");

            if (!stock.Any())
            {
                stock.Add(new Te(1, "Té Matcha", "Japón", 50.5m, 1000, true, "Verde"));
                stock.Add(new Te(2, "Té Chai", "India", 32.00m, 10000, false, "Negro"));
                stock.Add(new Te(2, "Té Puerh", "China", 22.00m, 10000, false, "Rojo"));
                stock.Add(new Te(2, "Té Oolong", "China", 45.00m, 1500, true, "Azul"));

                stock.Add(new Comida(2, "Tarta de Pistacho", "España", 5.0m, 350, false, "Dulce", true));
                stock.Add(new Comida(4, "Cookie de Avena", "España", 1.80m, 50, true, "Dulce", false));
                stock.Add(new Comida(5, "Sándwich Vegetal", "Francia", 3.50m, 80, false, "Salado", true));
                stock.Add(new Comida(6, "Wrap de Pollo", "Reino Unido", 2.20m, 120, true, "Salado", false));
                
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
}