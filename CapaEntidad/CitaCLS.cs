using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public class CitaCLS
    {
        [Key]
        [Column("ID")]
        public int idCita { get; set; }
        public int idPaciente { get; set; }
        public int idMedico { get; set; }
        public DateTime fechaHora { get; set; }
        public string estado { get; set; }
        public int BHABILITADO { get; set; }
    }


}