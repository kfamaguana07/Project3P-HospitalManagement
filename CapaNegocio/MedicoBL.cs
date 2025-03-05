using CapaDatos;
using CapaEntidad;
using Microsoft.AspNetCore.Identity;

namespace CapaNegocio
{
    public class MedicoBL
    {
        private readonly MedicoDAL _medicoDAL;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public MedicoBL(MedicoDAL medicoDAL, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AccountBL accountBL, ApplicationDbContext context)
        {
            _medicoDAL = medicoDAL;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public List<MedicoCLS> ListarMedico()
        {
            return _medicoDAL.ListarMedico();
        }

        public async Task GuardarMedico(MedicoCLS medico)
        {
            _medicoDAL.GuardarMedico(medico);
            // Si el medico es nuevo (idMedico == 0), crear el usuario en AspNetUsers
            if (medico.idMedico == 0)
            {
                var contrasena = AccountBL.GenerarContrasena(medico.nombre, medico.apellido, DateTime.Now);
                // Await the asynchronous method call
                await CrearUsuarioMedico(medico.email, contrasena);
            }
        }

        public MedicoCLS? RecuperarMedico(int idMedico)
        {
            return _medicoDAL.RecuperarMedico(idMedico);
        }

        public async Task EliminarMedico(int idMedico)
        {
            // deshabilitar la cuenta del usuario correspondiente
            var medico = _medicoDAL.RecuperarMedico(idMedico);
            if (medico != null && medico.email != null)
            {
                // Verifica si el usuario existe en AspNetUsers
                var user = await _userManager.FindByEmailAsync(medico.email);
                if (user != null)
                {
                    // Bloquea la cuenta para que no pueda hacer login
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.UtcNow.AddYears(100);  // El lockout por 100 años

                    // Guarda los cambios
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _medicoDAL.EliminarMedico(idMedico);  // Actualiza el estado de BHABILITADO a 0
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception("Error al deshabilitar la cuenta del medico: " + errors);
                    }
                }
            }
        }

        public int ObtenerIdDoctorDesdeEmail(string email)
        {
            var doctor = _context.MEDICOS
                     .Where(m => m.email == email)
                     .Select(m => m.idMedico)
                     .FirstOrDefault();

            if (doctor == 0)
            {
                throw new Exception("No se encontró un médico asociado al correo proporcionado.");
            }

            return doctor;
        }

        private async Task CrearUsuarioMedico(string email, string contrasena)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, contrasena);
            if (result.Succeeded)
            {
                // Asignar el rol de Doctor al usuario
                await _userManager.AddToRoleAsync(user, "Doctor");
            }
        }

        public async Task CrearCuentasParaMedicosExistentes()
        {
            if (!await _roleManager.RoleExistsAsync("Doctor"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Doctor"));
            }

            var medicos = _medicoDAL.ListarMedico();
            foreach (var medico in medicos)
            {
                if (medico.BHABILITADO == 1)
                {
                    var contrasena = AccountBL.GenerarContrasena(medico.nombre, medico.apellido, DateTime.Now);
                    await CrearUsuarioMedico(medico.email, contrasena);
                }
            }
        }


    }
}
