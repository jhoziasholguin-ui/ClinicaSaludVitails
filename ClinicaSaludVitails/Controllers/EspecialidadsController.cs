using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Data.Services;
using ClinicaSaludVitails.Models;
using ClinicaSaludVitails.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicaSaludVitails.Controllers
{
    [ValidarRol("Administrador")]
    public class EspecialidadsController : Controller
    {
        private readonly IEspecialidadService _especialidadService;

        public EspecialidadsController(
            IEspecialidadService especialidadService)
        {
            _especialidadService = especialidadService;
        }

        // GET: Especialidads
        public async Task<IActionResult> Index()
        {
            var especialidades =
                await _especialidadService.ObtenerTodasAsync();

            return View(especialidades);
        }

        // GET: Especialidads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var especialidad =
                await _especialidadService.ObtenerPorIdAsync(id.Value);

            if (especialidad == null)
                return NotFound();

            return View(especialidad);
        }

        // GET: Especialidads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especialidads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Nombre")] Especialidad especialidad)
        {
            if (!ModelState.IsValid)
                return View(especialidad);

            await _especialidadService.CrearAsync(especialidad);

            return RedirectToAction(nameof(Index));
        }

        // GET: Especialidads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var especialidad =
                await _especialidadService.ObtenerPorIdAsync(id.Value);

            if (especialidad == null)
                return NotFound();

            return View(especialidad);
        }

        // POST: Especialidads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Nombre")] Especialidad especialidad)
        {
            if (id != especialidad.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(especialidad);

            try
            {
                await _especialidadService.ActualizarAsync(especialidad);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _especialidadService.ExisteAsync(id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Especialidads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var especialidad =
                await _especialidadService.ObtenerPorIdAsync(id.Value);

            if (especialidad == null)
                return NotFound();

            return View(especialidad);
        }

        // POST: Especialidads/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var error = await _especialidadService.EliminarAsync(id);

                if (error != null)
                {
                    TempData["Error"] = error;
                }
                else
                {
                    TempData["Mensaje"] = "Especialidad eliminada correctamente.";
                }
            }
            catch (DbUpdateException)
            {
                TempData["Error"] =
                    "No se pudo eliminar la especialidad porque existen registros relacionados.";
            }
            catch
            {
                TempData["Error"] =
                    "Ocurrió un error inesperado al intentar eliminar la especialidad.";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}