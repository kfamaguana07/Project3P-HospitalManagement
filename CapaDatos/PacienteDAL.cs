using CapaEntidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos
{
    public class PacienteDAL
    {
        private readonly ApplicationDbContext _context;

        public PacienteDAL(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<PacienteCLS> ListarPaciente()
        {
            return _context.PACIENTES.FromSqlRaw("EXEC uspListarPacientes").ToList();
        }


        public List<PacienteCLS> ListarTodosPacientes()
        {
            return _context.PACIENTES.FromSqlRaw("EXEC uspListarTodosPacientes").ToList();
        }

        public List<PacienteCLS> ListarPacientesAsignados(int idDoctor)
        {
            return _context.PACIENTES.FromSqlRaw("EXEC uspListarPacientesAsignados @idDoctor", new SqlParameter("@idDoctor", idDoctor)).ToList();
        }
        public void GuardarPaciente(PacienteCLS paciente)
        {
            var idParam = new SqlParameter("@idPaciente", paciente.idPaciente);
            var nombreParam = new SqlParameter("@nombre", paciente.nombre);
            var apellidoParam = new SqlParameter("@apellido", paciente.apellido);
            var fechaNacimientoParam = new SqlParameter("@fechaNacimiento", paciente.fechaNacimiento);
            var telefonoParam = new SqlParameter("@telefono", paciente.telefono);
            var emailParam = new SqlParameter("@email", paciente.email);
            var direccionParam = new SqlParameter("@direccion", paciente.direccion);

            _context.Database.ExecuteSqlRaw("EXEC uspGuardarPaciente @idPaciente, @nombre, @apellido, @fechaNacimiento, @telefono, @email, @direccion",
                idParam, nombreParam, apellidoParam, fechaNacimientoParam, telefonoParam, emailParam, direccionParam);
        }

        public PacienteCLS? RecuperarPaciente(int idPaciente)
        {
            return _context.PACIENTES
                .FromSqlRaw("EXEC uspRecuperarPaciente @idPaciente", new SqlParameter("@idPaciente", idPaciente))
                .AsEnumerable()
                .FirstOrDefault();
        }

        public void EliminarPaciente(int idPaciente)
        {
            _context.Database.ExecuteSqlRaw("EXEC uspEliminarPaciente @idPaciente", new SqlParameter("@idPaciente", idPaciente));

        }
    }
}