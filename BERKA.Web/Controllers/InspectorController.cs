using BERKA.Models;
using BERKA.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BERKA.Web.Controllers
{
    public class InspectorController : Controller
    {
        private readonly HttpClient _http;
        public InspectorController(IHttpClientFactory factory)
            => _http = factory.CreateClient("ApiCliente");

        // GET: /Inspector
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var inspectores = await _http.GetFromJsonAsync<List<Inspector>>("inspector");
            return View(inspectores);
        }

        // GET: /Inspector/RegistrarInspector
        [HttpGet]
        public IActionResult RegistrarInspector()
        {
            // Puedes inicializar valores por defecto si quieres
            return View(new InspectorViewModel());
        }

        // POST: /Inspector/RegistrarInspector
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarInspector(InspectorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resp = await _http.PostAsJsonAsync("inspector", model);
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Inspector registrado exitosamente!"
                : "Error al registrar el inspector.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Inspector/EditarInspector/5
        [HttpGet]
        public async Task<IActionResult> EditarInspector(int id)
        {
            var insp = await _http.GetFromJsonAsync<Inspector>($"inspector/{id}");
            if (insp == null) return NotFound();

            var model = new InspectorViewModel
            {
                ID_Inspector = insp.ID_Inspector,
                Nombre = insp.Nombre,
                Apellido = insp.Apellido,
                Correo = insp.Correo,
                Telefono = insp.Telefono,
                Estacion = insp.Estacion,
                Contraseña = insp.Contraseña,
                Estado = insp.Estado
            };

            return View(model);
        }

        // POST: /Inspector/EditarInspector
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarInspector(InspectorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resp = await _http.PutAsJsonAsync($"inspector/{model.ID_Inspector}", model);
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Inspector actualizado con éxito!"
                : "Error al actualizar el inspector.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Inspector/EliminarInspector/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarInspector(int id)
        {
            var resp = await _http.DeleteAsync($"inspector/{id}");
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Inspector eliminado exitosamente!"
                : "Error al eliminar el inspector.";
            return RedirectToAction(nameof(Index));
        }
    }
}
