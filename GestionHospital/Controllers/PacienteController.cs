using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using System.Security.Claims;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class PacienteController : Controller
    {
        private readonly PacienteBL _pacienteBL;
        private readonly MedicoBL _medicoBL;

        public PacienteController(PacienteBL pacienteBL, MedicoBL medicoBL)
        {
            _pacienteBL = pacienteBL;
            _medicoBL = medicoBL;
        }

        [Authorize(Roles = "Admin,Staff,Doctor")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Patient")]
        public IActionResult InfoPersonal()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Staff,Doctor")]
        public List<PacienteCLS> ListarPaciente()
        {
            return _pacienteBL.ListarPaciente();
        }



        [Authorize(Roles = "Doctor")]
        public List<PacienteCLS> ListarPacientesAsignados()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Obtener el correo del usuario actual
            var idDoctor = _medicoBL.ObtenerIdDoctorDesdeEmail(userEmail); // Método para obtener el ID del médico desde el correo
            return _pacienteBL.ListarPacientesAsignados(idDoctor);
        }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<string> GuardarPaciente(PacienteCLS paciente)
        {
            await _pacienteBL.GuardarPaciente(paciente);
            string password = AccountBL.GenerarContrasena(paciente.nombre, paciente.apellido, paciente.fechaNacimiento);
            return password;
        }

        [Authorize(Roles = "Admin,Staff,Doctor,Patient")]
        public PacienteCLS? RecuperarPaciente(int idPaciente)
        {
            return _pacienteBL.RecuperarPaciente(idPaciente);
        }

        [Authorize(Roles = "Admin,Patient")]
        public int ObtenerIdPacienteActual()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            return _pacienteBL.ObtenerIdPacienteDesdeEmail(userEmail);
        }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> EliminarPaciente(int idPaciente)
        {
            try
            {
                await _pacienteBL.EliminarPaciente(idPaciente);
                return Ok("Paciente eliminado con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al eliminar el paciente: " + ex.Message);
            }
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearCuentasParaPacientesExistentes()
        {
            // Crear cuentas para pacientes existentes desde la capa de negocio
            await _pacienteBL.CrearCuentasParaPacientesExistentes();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeshabilitarCuentasDePacientesInhabilitados()
        {
            // Deshabilitar cuentas de pacientes inhabilitados desde la capa de negocio
            await _pacienteBL.DeshabilitarCuentasDePacientesInhabilitados();
            return Ok("Cuentas deshabilitadas con éxito");
        }
    }
}