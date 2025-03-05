using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CitaBL
    {
        private readonly CitaDAL _citaDAL;

        public CitaBL(CitaDAL citaDAL)
        {
            _citaDAL = citaDAL;
        }

        public List<CitaViewCLS> ListarCita()
        {
            return _citaDAL.ListarCita();
        }

        public List<CitaViewCLS> ListarCitasMedico(int idMedico)
        {
            return _citaDAL.ListarCitasMedico(idMedico);
        }

        public List<CitaViewCLS> ListarCitasPaciente(int idPaciente)
        {
            return _citaDAL.ListarCitasPaciente(idPaciente);
        }


        public void GuardarCita(CitaCLS cita)
        {
            _citaDAL.GuardarCita(cita);
        }

        public CitaCLS? RecuperarCita(int idCita)
        {
            return _citaDAL.RecuperarCita(idCita);
        }

        public void EliminarCita(int idCita)
        {
            _citaDAL.EliminarCita(idCita);
        }


    }
}