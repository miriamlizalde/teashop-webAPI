using System;

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
    }
}