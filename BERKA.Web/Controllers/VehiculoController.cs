using BERKA.Models;
using BERKA.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BERKA.Web.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly HttpClient _http;
        public VehiculoController(IHttpClientFactory factory)
            => _http = factory.CreateClient("ApiCliente");

        // GET: /Vehiculo
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vehiculos = await _http.GetFromJsonAsync<List<Vehiculo>>("vehiculo");
            return View(vehiculos);
        }

        // GET: /Vehiculo/RegistrarVehiculo
        [HttpGet]
        public async Task<IActionResult> RegistrarVehiculo()
        {
            ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
            return View(new VehiculoViewModel());
        }

        // POST: /Vehiculo/RegistrarVehiculo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarVehiculo(VehiculoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
                return View(model);
            }

            var resp = await _http.PostAsJsonAsync("vehiculo", model);
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Vehículo registrado exitosamente!"
                : "Error al registrar el vehículo.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /Vehiculo/EditarVehiculo/5
        [HttpGet]
        public async Task<IActionResult> EditarVehiculo(int id)
        {
            var veh = await _http.GetFromJsonAsync<Vehiculo>($"vehiculo/{id}");
            if (veh == null) return NotFound();

            var model = new VehiculoViewModel
            {
                ID_Vehiculo = veh.ID_Vehiculo,
                Marca = veh.Marca,
                Modelo = veh.Modelo,
                Categoria = veh.Categoria,
                Color = veh.Color,
                Año = veh.Año,
                Placa = veh.Placa,
                Tip_Combustible = veh.tip_Combustible,
                Kilometraje = veh.Kilometraje,
                ID_Cliente = veh.ID_Cliente
            };

            ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
            return View(model);
        }

        // POST: /Vehiculo/EditarVehiculo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarVehiculo(VehiculoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
                return View(model);
            }

            var resp = await _http.PutAsJsonAsync($"vehiculo/{model.ID_Vehiculo}", model);
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Vehículo actualizado con éxito!"
                : "Error al actualizar el vehículo.";

            return RedirectToAction(nameof(Index));
        }

        // POST: /Vehiculo/EliminarVehiculo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarVehiculo(int id)
        {
            var resp = await _http.DeleteAsync($"vehiculo/{id}");
            TempData["mensaje"] = resp.IsSuccessStatusCode
                ? "¡Vehículo eliminado exitosamente!"
                : "Error al eliminar el vehículo.";
            return RedirectToAction(nameof(Index));
        }
    }
}
