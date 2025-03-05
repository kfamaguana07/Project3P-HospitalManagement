using CapaEntidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos
{
    public class MedicoDAL
    {
        private readonly ApplicationDbContext _context;

        public MedicoDAL(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<MedicoCLS> ListarMedico()
        {
            return _context.MEDICOS.FromSqlRaw("EXEC uspListarMedicos").ToList();
        }

        public void GuardarMedico(MedicoCLS medico)
        {
            var idParam = new SqlParameter("@idMedico", medico.idMedico);
            var nombreParam = new SqlParameter("@nombre", medico.nombre);
            var apellidoParam = new SqlParameter("@apellido", medico.apellido);
            var telefonoParam = new SqlParameter("@telefono", medico.telefono);
            var emailParam = new SqlParameter("@email", medico.email);
            _context.Database.ExecuteSqlRaw("EXEC uspGuardarMedico @idMedico, @nombre, @apellido, @telefono, @email",
                idParam, nombreParam, apellidoParam, telefonoParam, emailParam);
        }

        public MedicoCLS? RecuperarMedico(int idMedico)
        {
            return _context.MEDICOS
                .FromSqlRaw("EXEC uspRecuperarMedico @idMedico", new SqlParameter("@idMedico", idMedico))
                .AsEnumerable()
                .FirstOrDefault();
        }

        public void EliminarMedico(int idMedico)
        {
            _context.Database.ExecuteSqlRaw("EXEC uspEliminarMedico @idMedico", new SqlParameter("@idMedico", idMedico));
        }
    }
}
