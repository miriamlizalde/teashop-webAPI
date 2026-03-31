using System;
using System.ComponentModel.DataAnnotations;

namespace TeaShop.Models;

public class Usuario
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
    public DateTime FechaRegistro { get; set; }
    public decimal Saldo { get; set; } = 0;
    public bool EsSocio { get; set; }
    public string Rol { get; set; }   

    public List<Pedido> Pedidos { get; set; } = new List<Pedido>();  
}