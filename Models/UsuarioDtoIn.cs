using System.ComponentModel.DataAnnotations;

namespace TeaShop.Models; 

public class UserDtoIn
    {
        [Required]
        public string Nombre { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "La contraseña debe ser de 15 caracteres")]
        public string Password { get; set; }
        public bool EsSocio { get; set; }
}

