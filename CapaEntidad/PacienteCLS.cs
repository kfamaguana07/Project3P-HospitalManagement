using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public class PacienteCLS
    {
        [Key]
        [Column("ID")] // indicar explícitamente a EF Core que la propiedad idPaciente se mapea a la columna ID
        public int idPaciente { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string direccion { get; set; }
        public int BHABILITADO { get; set; }
    }
}

