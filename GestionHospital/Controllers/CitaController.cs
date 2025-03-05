using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class CitaController : Controller
    {
        private readonly CitaBL _citaBL;
        private readonly MedicoBL _medicoBL;
        private readonly PacienteBL _pacienteBL;
        public CitaController(CitaBL citaBL,MedicoBL medicoBL, PacienteBL pacienteBL)
        {
            _citaBL = citaBL;
            _medicoBL = medicoBL;
            _pacienteBL = pacienteBL;
        }
        [Authorize(Roles = "Admin, Patient, Doctor, Staff")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        public List<CitaViewCLS> ListarCitas()
        {
            List<CitaViewCLS> citas = _citaBL.ListarCita();
            return citas;
        }

        [Authorize(Roles = "Admin,Doctor")]
        public List<CitaViewCLS> ListarCitasMedico()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var idMedico = _medicoBL.ObtenerIdDoctorDesdeEmail(userEmail);
            List<CitaViewCLS> citas = _citaBL.ListarCitasMedico(idMedico);
            return citas;
        }

        [Authorize(Roles = "Admin,Patient")]
        public List<CitaViewCLS> ListarCitasPaciente()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var idPaciente = _pacienteBL.ObtenerIdPacienteDesdeEmail(userEmail);
            List<CitaViewCLS> citas = _citaBL.ListarCitasPaciente(idPaciente);
            return citas;
        }


        [Authorize(Roles = "Admin, Staff, Doctor")]
        public void GuardarCita(CitaCLS cita)
        {
            _citaBL.GuardarCita(cita);
        }

        [Authorize(Roles = "Admin, Staff, Doctor")]
        public CitaCLS? RecuperarCita(int idCita)
        {
            return _citaBL.RecuperarCita(idCita);
        }

        [Authorize(Roles = "Admin, Staff, Doctor")]
        public void EliminarCita(int idCita)
        {
            _citaBL.EliminarCita(idCita);
        }

    }
}