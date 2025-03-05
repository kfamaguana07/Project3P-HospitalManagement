using CapaDatos;
using CapaEntidad;
using GestionHospital.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionHospital.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PacienteDAL _pacienteDAL;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, PacienteDAL pacienteDAL)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _pacienteDAL = pacienteDAL;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Ensure the "Patient" role exists
                    if (!await _roleManager.RoleExistsAsync("Patient"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Patient"));
                    }

                    // Assign the "Patient" role to the user
                    await _userManager.AddToRoleAsync(user, "Patient");

                    // Save patient data in the PACIENTES table
                    var paciente = new PacienteCLS
                    {
                        nombre = model.Nombre,
                        apellido = model.Apellido,
                        fechaNacimiento = model.FechaNacimiento,
                        telefono = model.Telefono,
                        email = model.Email,
                        direccion = model.Direccion,
                        BHABILITADO = 1
                    };
                    _pacienteDAL.GuardarPaciente(paciente);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }



    }
}
