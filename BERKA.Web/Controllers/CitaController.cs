using BERKA.Models;
using BERKA.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BERKA.Web.Controllers
{
    public class CitaController : Controller
    {
        private readonly HttpClient _http;
        public CitaController(IHttpClientFactory factory)
            => _http = factory.CreateClient("ApiCliente");

        // GET: /Cita
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var citas = await _http.GetFromJsonAsync<List<Cita>>("cita");
            return View(citas);
        }

        // GET: /Cita/RegistrarCita
        [HttpGet]
        public async Task<IActionResult> RegistrarCita()
        {
            ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
            ViewBag.Inspectores = await _http.GetFromJsonAsync<List<Inspector>>("inspector");
            // Vehicles serán cargados por AJAX
            return View(new CitaViewModel());
        }

        // POST: /Cita/RegistrarCita
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarCita(CitaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
                ViewBag.Inspectores = await _http.GetFromJsonAsync<List<Inspector>>("inspector");
                return View(model);
            }

            var resp = await _http.PostAsJsonAsync("cita", model);
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Cita registrada con éxito!"
                : "Error al crear la cita.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Cita/EditarCita/{id}
        [HttpGet]
        public async Task<IActionResult> EditarCita(int id)
        {
            var cita = await _http.GetFromJsonAsync<Cita>($"cita/{id}");
            if (cita == null) return NotFound();

            // Mapea a ViewModel
            var model = new CitaViewModel
            {
                ID_Cita = cita.ID_Cita,
                Fecha = cita.Fecha,
                Hora = cita.Hora,
                Estado = cita.Estado,
                ID_Cliente = cita.ID_Cliente,
                ID_Vehiculo = cita.ID_Vehiculo,
                ID_Inspector = cita.ID_Inspector
            };

            ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
            ViewBag.Inspectores = await _http.GetFromJsonAsync<List<Inspector>>("inspector");
            // Vehículos del cliente actual
            var todosVeh = await _http.GetFromJsonAsync<List<Vehiculo>>("vehiculo");
            ViewBag.Vehiculos = todosVeh.Where(v => v.Cliente?.ID_Cliente == model.ID_Cliente).ToList();

            return View(model);
        }

        // POST: /Cita/EditarCita
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCita(CitaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
                ViewBag.Inspectores = await _http.GetFromJsonAsync<List<Inspector>>("inspector");
                var todosVeh = await _http.GetFromJsonAsync<List<Vehiculo>>("vehiculo");
                ViewBag.Vehiculos = todosVeh.Where(v => v.Cliente?.ID_Cliente == model.ID_Cliente).ToList();
                return View(model);
            }

            var resp = await _http.PutAsJsonAsync($"cita/{model.ID_Cita}", model);
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Cita actualizada con éxito!"
                : "Error al actualizar la cita.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cita/EliminarCita/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCita(int id)
        {
            var resp = await _http.DeleteAsync($"cita/{id}");
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Cita eliminada exitosamente!"
                : "Error al eliminar la cita.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Cita/VehiculosPorCliente?id=5
        [HttpGet]
        public async Task<IActionResult> VehiculosPorCliente(int id)
        {
            var todos = await _http.GetFromJsonAsync<List<Vehiculo>>("vehiculo");
            var filtrados = todos.Where(v => v.Cliente?.ID_Cliente == id).ToList();
            return Json(filtrados);
        }
    }
}
