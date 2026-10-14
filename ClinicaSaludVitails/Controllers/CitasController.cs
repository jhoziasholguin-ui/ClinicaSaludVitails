
using ClinicaSaludVitails.Interfaces;
using ClinicaSaludVitails.Models;
using ClinicaSaludVitails.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicaSaludVitails.Controllers
{
    
    public class CitasController : Controller
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [ValidarRol("Administrador", "Recepcionista")]
        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var citas = await _citaService.ObtenerTodasAsync();
            return View(citas);
        }

        // GET: Citas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var cita = await _citaService.ObtenerPorIdAsync(id.Value);

            if (cita == null)
                return NotFound();

            return View(cita);
        }

        // GET: Citas/Create
        [ValidarRol("Administrador", "Recepcionista")]
        public async Task<IActionResult> Create()
        {
            await CargarListasAsync();
            return View();
        }

        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,PacienteId,MedicoId,Fecha,Hora,Estado")]
            Cita cita)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasAsync(cita);
                return View(cita);
            }

            bool creado = await _citaService.CrearAsync(cita);

            if (!creado)
            {
                ModelState.AddModelError(
                    "",
                    "El médico ya tiene una cita programada para esa fecha y hora.");

                await CargarListasAsync(cita);
                return View(cita);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var cita = await _citaService.ObtenerPorIdAsync(id.Value);

            if (cita == null)
                return NotFound();

            await CargarListasAsync(cita);

            return View(cita);
        }

        // POST: Citas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,PacienteId,MedicoId,Fecha,Hora,Estado")]
            Cita cita)
        {
            if (id != cita.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarListasAsync(cita);
                return View(cita);
            }

            // Comprobamos que la cita exista antes de actualizarla.
            bool existe = await _citaService.ExisteAsync(id);

            if (!existe)
                return NotFound();

            bool actualizado = await _citaService.ActualizarAsync(cita);

            if (!actualizado)
            {
                ModelState.AddModelError(
                    "",
                    "El médico ya tiene otra cita programada para esa fecha y hora.");

                await CargarListasAsync(cita);
                return View(cita);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var cita = await _citaService.ObtenerPorIdAsync(id.Value);

            if (cita == null)
                return NotFound();

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool eliminado = await _citaService.EliminarAsync(id);

            if (!eliminado)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: Citas/ExportarExcel
        public async Task<IActionResult> ExportarExcel()
        {
            var citas = await _citaService.ObtenerTodasAsync();

            Response.Headers["Content-Disposition"] =
                "attachment; filename=Reporte_Citas_Vitails.xls";

            Response.ContentType = "application/vnd.ms-excel";

            return PartialView("_ReporteExcel", citas);
        }

        // GET: Citas/MiAgenda
        [ValidarRol("Medico", "Administrador")]
        public async Task<IActionResult> MiAgenda()
        {
            var correo = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrWhiteSpace(correo))
                return RedirectToAction("Login", "Account");

            var citas = await _citaService
                .ObtenerAgendaPorCorreoAsync(correo);

            return View(citas);
        }

        // Preparamos las listas desplegables para las vistas.
        private async Task CargarListasAsync(Cita? cita = null)
        {
            var medicos = await _citaService.ObtenerMedicosAsync();
            var pacientes = await _citaService.ObtenerPacientesAsync();

            ViewData["MedicoId"] = new SelectList(
                medicos,
                "Id",
                "Apellidos",
                cita?.MedicoId);

            ViewData["PacienteId"] = new SelectList(
                pacientes,
                "Id",
                "Apellidos",
                cita?.PacienteId);
        }
    }
}
