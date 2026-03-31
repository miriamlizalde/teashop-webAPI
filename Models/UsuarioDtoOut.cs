using System.ComponentModel.DataAnnotations;

namespace TeaShop.Models;

public class UsuarioDtoOut
    {
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Rol { get; set; }
        public decimal Saldo { get; set; }
        public bool EsSocio { get; set; }
}

