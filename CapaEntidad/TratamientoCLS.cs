using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class TratamientoCLS
    {
        [Key]
        [Column("ID")] // indicar explícitamente a EF Core que la propiedad idTratamiento se mapea a la columna ID
        public int idTratamiento { get; set; }
        [Column("PACIENTEID")] // Mapea idPaciente a la columna PACIENTEID
        public int idPaciente { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha { get; set; }
        public decimal costo { get; set; }
        public int BHABILITADO { get; set; }
    }
}
