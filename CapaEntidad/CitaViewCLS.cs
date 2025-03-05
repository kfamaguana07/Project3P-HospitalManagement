using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public class CitaViewCLS
    {

        [Key]
        [Column("ID")]
        public int idCita { get; set; }
        public int idPaciente { get; set; }
        public string nombreCompletoPaciente { get; set; }
        public int idMedico { get; set; }
        public string nombreCompletoMedico { get; set; }
        public DateTime fechaHora { get; set; }
        public string estado { get; set; }
        public int BHABILITADO { get; set; }
    }



}