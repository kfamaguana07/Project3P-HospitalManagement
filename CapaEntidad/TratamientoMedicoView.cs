using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public class TratamientoMedicoViewCLS
    {

        [Key]
        [Column("ID")] // indicar explícitamente a EF Core que la propiedad idTratamiento se mapea a la columna ID
        public int idTratamiento { get; set; }
        public int idPaciente { get; set; }
        public string nombreCompletoPaciente { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha { get; set; }
        public decimal costo { get; set; }
        public int BHABILITADO { get; set; }
        public int idMedico { get; set; }
    }



}