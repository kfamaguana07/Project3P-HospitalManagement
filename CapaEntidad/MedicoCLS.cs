using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public class MedicoCLS
    {
        [Key]
        [Column("ID")] // indicar explícitamente a EF Core que la propiedad idMedico se mapea a la columna ID
        public int idMedico { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public int BHABILITADO { get; set; }
    }
}

