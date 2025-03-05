using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using System.Security.Claims;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class MedicoController : Controller
    {
        private readonly MedicoBL _medicoBL;

        public MedicoController(MedicoBL medicoBL)
        {
            _medicoBL = medicoBL;
        }

        [Authorize(Roles = "Admin, Staff, Doctor")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        public List<MedicoCLS> ListarMedico()
        {
            return _medicoBL.ListarMedico();

        }

        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult InfoPersonal()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<string> GuardarMedico(MedicoCLS medico)
        {
            await _medicoBL.GuardarMedico(medico);
            string password = AccountBL.GenerarContrasena(medico.nombre, medico.apellido, DateTime.Now);
            return password; 

        }

        [Authorize(Roles = "Admin, Doctor")]
        public MedicoCLS? RecuperarMedico(int idMedico)
        {
            return _medicoBL.RecuperarMedico(idMedico);
        }

        [Authorize(Roles = "Admin, Doctor")]
        public int ObtenerIdMedicoActual()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            return _medicoBL.ObtenerIdDoctorDesdeEmail(userEmail);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EliminarMedico(int idMedico)
        {
            try
            {
                await _medicoBL.EliminarMedico(idMedico);
                return Ok("Médico deshabilitado con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al deshabilitar el médico: " + ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearCuentasParaMedicosExistentes()
        {
            try
            {
                await _medicoBL.CrearCuentasParaMedicosExistentes();
                return Ok("Cuentas de médicos creadas con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear cuentas para médicos: " + ex.Message);
            }
        }

    }
}