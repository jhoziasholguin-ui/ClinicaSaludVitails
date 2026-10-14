
using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaSaludVitails.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(
            IAccountService accountService,
            IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string correo,
            string password)
        {
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Ingrese su correo y contraseña.");

                return View();
            }

            var usuario = await _accountService
                .AutenticarAsync(correo, password);

            if (usuario == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Credenciales incorrectas.");

                return View();
            }

            // La sesión sigue siendo responsabilidad del controlador.
            HttpContext.Session.SetString("UserRole", usuario.Rol);
            HttpContext.Session.SetString("UserEmail", usuario.Correo);

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            [Bind("Correo,Password,Rol")] Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            bool permitirAdmin = _configuration.GetValue<bool>(
                "AppSettings:PermitirRegistroAdmin");

            string? error = await _accountService
                .RegistrarAsync(usuario, permitirAdmin);

            if (error != null)
            {
                // Mostrar el error en el resumen de validación.
                ModelState.AddModelError(string.Empty, error);
                return View(usuario);
            }

            return RedirectToAction(nameof(Login));
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Login));
        }
    }
}
