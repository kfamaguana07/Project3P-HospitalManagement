using CapaEntidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos
{
    public class FacturacionDAL
    {
        private readonly ApplicationDbContext _context;

        public FacturacionDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<FacturacionViewCLS> ListarFacturacion()
        {
            return _context.FacturacionResultados.ToList();
        }

        public List<FacturacionCLS> ListarFacturacionPaciente(int idPaciente)
        {
            return _context.FACTURACION.Where(c => c.idPaciente == idPaciente).ToList();
        }

        public void GuardarFacturacion(FacturacionCLS facturacion)
        {
            var idFacturacionParam = new SqlParameter("@idFacturacion", facturacion.idFacturacion);
            var idPacienteParam = new SqlParameter("@idPaciente", facturacion.idPaciente);
            var montoParam = new SqlParameter("@monto", facturacion.monto);
            var metodoPagoParam = new SqlParameter("@metodoPago", facturacion.metodoPago);
            var fechaPagoParam = new SqlParameter("@fechaPago", facturacion.fechaPago);

            _context.Database.ExecuteSqlRaw("EXEC uspGuardarFacturacion @idFacturacion, @idPaciente, @monto, @metodoPago, @fechaPago",
                idFacturacionParam, idPacienteParam, montoParam, metodoPagoParam, fechaPagoParam);
        }

        public FacturacionCLS? RecuperarFacturacion(int idFacturacion)
        {
            return _context.FACTURACION
                .FromSqlRaw("EXEC uspRecuperarFacturacion @idFacturacion", new SqlParameter("@idFacturacion", idFacturacion))
                .AsEnumerable()
                .FirstOrDefault();
        }

        public void EliminarFacturacion(int idFacturacion)
        {
            _context.Database.ExecuteSqlRaw("EXEC uspEliminarFacturacion @idFacturacion", new SqlParameter("@idFacturacion", idFacturacion));
        }
    }
}
