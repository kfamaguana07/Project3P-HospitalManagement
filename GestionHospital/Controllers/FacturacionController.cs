using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using System.Security.Claims;


namespace GestionHospital.Controllers
{
    [Authorize]
    public class FacturacionController : Controller
    {
        private readonly FacturacionBL _facturacionBL;
        private readonly PacienteBL _pacienteBL;


        public FacturacionController(FacturacionBL facturacionBL, PacienteBL pacienteBL)
        {
            _facturacionBL = facturacionBL;
            _pacienteBL = pacienteBL;
        }

        [Authorize(Roles = "Admin, Staff, Patient")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        public List<FacturacionViewCLS> ListarFacturacion()
        {
            List<FacturacionViewCLS> facturacion = _facturacionBL.ListarFacturacion();
            return facturacion;
        }

        [Authorize(Roles = "Admin, Patient")]
        public List<FacturacionCLS> ListarFacturacionPaciente()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Obtener el correo del usuario actual
            var idPaciente = _pacienteBL.ObtenerIdPacienteDesdeEmail(userEmail); // Método para obtener el ID del médico desde el correo
            List<FacturacionCLS> facturasPaciente = _facturacionBL.ListarFacturacionPaciente(idPaciente);
            return facturasPaciente;
        }

        [Authorize(Roles = "Admin, Staff")]
        public FacturacionCLS? RecuperarFacturacion(int idFacturacion)
        {
            return _facturacionBL.RecuperarFacturacion(idFacturacion);
        }

        [Authorize(Roles = "Admin, Staff")]
        public void EliminarFacturacion(int idFacturacion)
        {
            _facturacionBL.EliminarFacturacion(idFacturacion);
        }

        [Authorize(Roles = "Admin, Staff")]
        public void GuardarFacturacion(FacturacionCLS facturacion)
        {
            _facturacionBL.GuardarFacturacion(facturacion);
        }
    }
}
