using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using System.Security.Claims;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class TratamientoController : Controller
    {
        private readonly TratamientoBL _tratamientoBL;
        private readonly MedicoBL _medicoBL;


        public TratamientoController(TratamientoBL tratamientoBL, MedicoBL medicoBL)
        {
            _tratamientoBL = tratamientoBL;
            _medicoBL = medicoBL;

        }

        [Authorize(Roles = "Admin, Staff, Doctor")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        public List<TratamientoViewCLS> ListarTratamiento()
        {
            return _tratamientoBL.ListarTratamiento();

        }

        [Authorize(Roles = "Admin, Doctor")]
        public List<TratamientoMedicoViewCLS> ListarTratamientoMedico()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var idMedico = _medicoBL.ObtenerIdDoctorDesdeEmail(userEmail);
            return _tratamientoBL.ListarTratamientosMedico(idMedico);

        }


        [Authorize(Roles = "Admin, Doctor, Staff")]
        public TratamientoCLS? RecuperarTratamiento(int idTratamiento)
        {
            return _tratamientoBL.RecuperarTratamiento(idTratamiento);
        }

        [Authorize(Roles = "Admin, Doctor, Staff")]
        public void EliminarTratamiento(int idTratamiento)
        {
            _tratamientoBL.EliminarTratamiento(idTratamiento);   
        }

        [Authorize(Roles = "Admin, Doctor, Staff")]
        public void GuardarTratamiento(TratamientoCLS tratamiento)
        {
            _tratamientoBL.GuardarTratamiento(tratamiento);
        }


    }
}
