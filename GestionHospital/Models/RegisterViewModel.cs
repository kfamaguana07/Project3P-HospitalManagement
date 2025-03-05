using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación de la contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
    }
}
