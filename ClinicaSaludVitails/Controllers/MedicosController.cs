using ClinicaSaludVitails.Core.Interfaces;
using ClinicaSaludVitails.Models;
using ClinicaSaludVitails.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaSaludVitails.ViewModels;

namespace ClinicaSaludVitails.Controllers
{
    [ValidarRol("Administrador")]
    public class MedicosController : Controller
    {
        private readonly IMedicoService _medicoService;

        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            var medicos = await _medicoService.ObtenerTodosAsync();

            return View(medicos);
        }

        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var medico = await _medicoService.ObtenerPorIdAsync(id.Value);

            if (medico == null)
                return NotFound();

            return View(medico);
        }

        // GET: Medicos/Create

        public async Task<IActionResult> Create()
        {
            await CargarEspecialidadesAsync();
            return View(new MedicoCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicoCreateViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                await CargarEspecialidadesAsync(modelo.EspecialidadId);
                return View(modelo);
            }

            var medico = new Medico
            {
                Nombres = modelo.Nombres,
                Apellidos = modelo.Apellidos,
                Correo = modelo.Correo,
                CMP = modelo.CMP,
                EspecialidadId = modelo.EspecialidadId
            };

            try
            {
                var error = await _medicoService.CrearConCuentaAsync(
                    medico,
                    modelo.CrearCuenta,
                    modelo.Password);

                if (error != null)
                {
                    ModelState.AddModelError(string.Empty, error);
                    await CargarEspecialidadesAsync(modelo.EspecialidadId);
                    return View(modelo);
                }

                TempData["Mensaje"] = modelo.CrearCuenta
                    ? "Médico y cuenta de acceso creados correctamente."
                    : "Ficha del médico creada correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No se pudo guardar el médico. Verifica los datos e inténtalo nuevamente.");

                await CargarEspecialidadesAsync(modelo.EspecialidadId);
                return View(modelo);
            }
        }


      
// GET: Medicos/Edit/5
public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var medico = await _medicoService.ObtenerPorIdAsync(id.Value);

            if (medico == null)
                return NotFound();

            var modelo = new MedicoEditViewModel
            {
                Id = medico.Id,
                Nombres = medico.Nombres,
                Apellidos = medico.Apellidos,
                Correo = medico.Correo,
                CMP = medico.CMP,
                EspecialidadId = medico.EspecialidadId,
                TieneCuenta = medico.Usuario != null
            };

            await CargarEspecialidadesAsync(modelo.EspecialidadId);

            return View(modelo);
        }

        // POST: Medicos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            MedicoEditViewModel modelo)
        {
            if (id != modelo.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                var actual = await _medicoService.ObtenerPorIdAsync(id);

                if (actual == null)
                    return NotFound();

                modelo.TieneCuenta = actual.Usuario != null;
                await CargarEspecialidadesAsync(modelo.EspecialidadId);

                return View(modelo);
            }

            try
            {
                var medico = new Medico
                {
                    Id = modelo.Id,
                    Nombres = modelo.Nombres,
                    Apellidos = modelo.Apellidos,
                    Correo = modelo.Correo,
                    CMP = modelo.CMP,
                    EspecialidadId = modelo.EspecialidadId
                };

                await _medicoService.ActualizarAsync(medico);

                if (modelo.CrearCuenta)
                {
                    var error = await _medicoService.CrearCuentaParaMedicoAsync(
                        id,
                        modelo.Password ?? string.Empty);

                    if (error != null)
                    {
                        ModelState.AddModelError(string.Empty, error);

                        var actual = await _medicoService.ObtenerPorIdAsync(id);

                        if (actual == null)
                            return NotFound();

                        modelo.TieneCuenta = actual.Usuario != null;
                        await CargarEspecialidadesAsync(modelo.EspecialidadId);

                        return View(modelo);
                    }
                }

                TempData["Mensaje"] = modelo.CrearCuenta
                    ? "Médico actualizado y cuenta creada correctamente."
                    : "Médico actualizado correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No se pudieron guardar los cambios. Verifica los datos e inténtalo nuevamente.");

                var actual = await _medicoService.ObtenerPorIdAsync(id);

                if (actual == null)
                    return NotFound();

                modelo.TieneCuenta = actual.Usuario != null;
                await CargarEspecialidadesAsync(modelo.EspecialidadId);

                return View(modelo);
            }
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var medico = await _medicoService.ObtenerPorIdAsync(id.Value);

            if (medico == null)
                return NotFound();

            return View(medico);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var error = await _medicoService.EliminarAsync(id);

                if (error != null)
                {
                    TempData["Error"] = error;
                }
                else
                {
                    TempData["Mensaje"] = "Médico eliminado correctamente.";
                }
            }
            catch
            {
                TempData["Error"] =
                    "No se pudo eliminar el médico debido a una restricción de la base de datos.";
            }

            return RedirectToAction(nameof(Index));
        }


        // Carga las especialidades para los formularios
        private async Task CargarEspecialidadesAsync(
            int? especialidadId = null)
        {
            var especialidades =
                await _medicoService.ObtenerEspecialidadesAsync();

            ViewData["EspecialidadId"] = new SelectList(
                especialidades,
                "Id",
                "Nombre",
                especialidadId);
        }
    }
}