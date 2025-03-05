using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos
{
    public class TratamientoDAL
    {
        private readonly ApplicationDbContext _context;

        public TratamientoDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TratamientoViewCLS> ListarTratamiento()
        {
            return _context.TratamientosResultado.ToList();
        }

        public List<TratamientoMedicoViewCLS> ListarTratamientosMedico(int idMedico)
        {
            var tratamientos = _context.TratamientosMedico
                .Where(t => t.idMedico == idMedico) // Filtra por el ID del médico
                .ToList();
            return tratamientos;
        }
        public void GuardarTratamiento(TratamientoCLS tratamiento)
        {
            var idTratamientoParam = new SqlParameter("@idTratamiento", tratamiento.idTratamiento);
            var idPacienteParam = new SqlParameter("@idPaciente", tratamiento.idPaciente);
            var descripcionParam = new SqlParameter("@descripcion", tratamiento.descripcion);
            var fechaParam = new SqlParameter("@fecha", tratamiento.fecha);
            var costoParam = new SqlParameter("@costo", tratamiento.costo);

            _context.Database.ExecuteSqlRaw("EXEC uspGuardarTratamiento @idTratamiento, @idPaciente, @descripcion, @fecha, @costo",
                idTratamientoParam, idPacienteParam, descripcionParam, fechaParam, costoParam);
        }

        public TratamientoCLS? RecuperarTratamiento(int idTratamiento)
        {
            return _context.TRATAMIENTOS
                .FromSqlRaw("EXEC uspRecuperarTratamiento @idTratamiento", new SqlParameter("@idTratamiento", idTratamiento))
                .AsEnumerable()
                .FirstOrDefault();
        }

        public void EliminarTratamiento(int idTratamiento)
        {
            _context.Database.ExecuteSqlRaw("EXEC uspEliminarTratamiento @idTratamiento", new SqlParameter("@idTratamiento", idTratamiento));
        }
    }
}
