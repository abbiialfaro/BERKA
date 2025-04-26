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
            Console.WriteLine("👉 Entré al POST RegistrarCita, ModelState: " + ModelState.IsValid);

            if (!ModelState.IsValid)
            {
                // DEBUG: escribe errores en consola
                foreach (var e in ModelState.Values.SelectMany(v => v.Errors))
                    Console.WriteLine("ModelError: " + e.ErrorMessage);
                // Recarga datos para selects
                ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
                ViewBag.Inspectores = await _http.GetFromJsonAsync<List<Inspector>>("inspector");
                return View(model);
            }

            var resp = await _http.PostAsJsonAsync("cita", model);
            Console.WriteLine("👉 API POST /api/cita: " + resp.StatusCode);
            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al crear cita en API: " + resp.StatusCode);
                ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
                ViewBag.Inspectores = await _http.GetFromJsonAsync<List<Inspector>>("inspector");
                return View(model);
            }

            TempData["mensaje"] = "¡Cita registrada con éxito!";
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
            // 1. Trae todos los vehículos del API
            var todos = await _http.GetFromJsonAsync<List<Vehiculo>>("vehiculo");
            if (todos == null) return Json(new List<Vehiculo>());

            // 2. Filtra por ID_Cliente
            var filtrados = todos
                .Where(v => v.ID_Cliente == id)
                .Select(v => new {
                    ID_Vehiculo = v.ID_Vehiculo,
                    Placa = v.Placa
                })
                .ToList();

            // 3. Devuelve JSON
            return Json(filtrados);
        }
    }
}
