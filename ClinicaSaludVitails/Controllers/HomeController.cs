
using System.Diagnostics;
using ClinicaSaludVitails.Models;
using ClinicaSaludVitails.Interfaces;
using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ClinicaSaludVitails.Util;

namespace ClinicaSaludVitails.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICitaService _citaService;
        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;

        public HomeController(
            ILogger<HomeController> logger,
            ICitaService citaService,
            IPacienteService pacienteService,
            IMedicoService medicoService)
        {
            _logger = logger;
            _citaService = citaService;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }


        public IActionResult Index()
        {
            // Página pública de bienvenida
            return View();
        }

        [ValidarRol("Administrador")]
        public async Task<IActionResult> Dashboard()
        {
            var citas = await _citaService.ObtenerTodasAsync();
            var pacientes = await _pacienteService.ObtenerTodosAsync();
            var medicos = await _medicoService.ObtenerTodosAsync();

            var modelo = new HomeDashboardViewModel
            {
                TotalCitas = citas.Count,
                CitasProgramadas = citas.Count(c => c.Estado == "Programada"),
                CitasAtendidas = citas.Count(c => c.Estado == "Atendida"),
                CitasCanceladas = citas.Count(c => c.Estado == "Cancelada"),
                TotalPacientes = pacientes.Count,
                TotalMedicos = medicos.Count
            };

            var hoy = DateTime.Today;

            modelo.Meses = Enumerable.Range(1, 12)
                .Select(m => new DateTime(hoy.Year, m, 1).ToString("MMM"))
                .ToList();

            modelo.CitasPorMes = Enumerable.Range(1, 12)
                .Select(m => citas.Count(c =>
                    c.Fecha.Year == hoy.Year &&
                    c.Fecha.Month == m))
                .ToList();

            modelo.Estados = new List<string>
    {
        "Programada", "Atendida", "Cancelada"
    };

            modelo.CitasPorEstado = new List<int>
    {
        modelo.CitasProgramadas,
        modelo.CitasAtendidas,
        modelo.CitasCanceladas
    };

            return View(modelo);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}
