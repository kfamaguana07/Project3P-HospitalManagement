using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace CapaEntidad
{
    public class FacturacionCLS
    {
        [Key]
        [Column("ID")] // indicar explícitamente a EF Core que la propiedad idFacturacion se mapea a la columna ID
        public int idFacturacion { get; set; }
        [Column("PACIENTEID")]
        public int idPaciente { get; set; }
        public decimal monto { get; set; }
        public string metodoPago { get; set; }
        public DateTime fechaPago { get; set; }
        public int BHABILITADO { get; set; }
    }
}
