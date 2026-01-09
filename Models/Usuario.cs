using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace TeaShop.Models

{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public decimal Saldo { get; set; } = 0;
        public bool EsSocio { get; set; }
        public bool EsAdmin { get; set; } = false;

        public Usuario(int id, string nombre, string email, string password, bool esSocio, decimal saldo = 0)
        {
            Id = id;
            Nombre = nombre;
            Email = email;
            Password = password;
            EsSocio = esSocio;
            FechaRegistro = DateTime.Now;
            Saldo = saldo;
        }

        public void MostrarDetalles() {
            string socioStatus = EsSocio ? "Sí" : "No";
            Console.WriteLine($"Usuario: {Nombre} | Email: {Email} | Socio: {socioStatus} | Saldo: ${Saldo:F2} | Fecha de Registro: {FechaRegistro.ToShortDateString()}");
        }

        public static void NuevoUsuario(List<Usuario> usuarios)
        {
            AnsiConsole.MarkupLine("[bold blue]--REGISTRAR USUARIO--[/]");
            int id = usuarios.Any() ? usuarios.Max(u => u.Id) + 1 : 1;

            string nombre = AnsiConsole.Ask<string>("Nombre del usuario:");
            string email = AnsiConsole.Ask<string>("Email:");
            string password = AnsiConsole.Ask<string>("Contraseña:");
            bool esSocio = AnsiConsole.Confirm("¿Es socio?");

            usuarios.Add(new Usuario(id, nombre, email, password, esSocio));
            AnsiConsole.MarkupLine("[green]Usuario creado con éxito.[/]");
        }
    }
}