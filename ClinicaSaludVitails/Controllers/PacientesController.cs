
using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Models;
using ClinicaSaludVitails.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaSaludVitails.Controllers
{
    [ValidarRol("Administrador", "Recepcionista")]
    public class PacientesController : Controller
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            var pacientes =
                await _pacienteService.ObtenerTodosAsync();

            return View(pacientes);
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente =
                await _pacienteService.ObtenerPorIdAsync(id.Value);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,DNI,Nombres,Apellidos,Telefono")]
            Paciente paciente)
        {
            if (!ModelState.IsValid)
                return View(paciente);

            // Validar que el DNI no esté registrado.
            if (await _pacienteService.ExisteDniAsync(paciente.DNI))
            {
                ModelState.AddModelError(
                    nameof(paciente.DNI),
                    "El número de DNI ingresado ya se encuentra registrado con otro paciente.");

                return View(paciente);
            }

            await _pacienteService.CrearAsync(paciente);

            return RedirectToAction(nameof(Index));
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente =
                await _pacienteService.ObtenerPorIdAsync(id.Value);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,DNI,Nombres,Apellidos,Telefono")]
            Paciente paciente)
        {
            if (id != paciente.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(paciente);

            // Excluir al paciente actual de la comprobación.
            if (await _pacienteService.ExisteDniAsync(
                paciente.DNI, paciente.Id))
            {
                ModelState.AddModelError(
                    nameof(paciente.DNI),
                    "Error: El DNI ingresado ya le pertenece a otro paciente registrado.");

                return View(paciente);
            }

            try
            {
                await _pacienteService.ActualizarAsync(paciente);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _pacienteService.ExisteAsync(id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente =
                await _pacienteService.ObtenerPorIdAsync(id.Value);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pacienteService.EliminarAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
