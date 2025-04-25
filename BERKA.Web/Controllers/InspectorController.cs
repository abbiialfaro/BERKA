using BERKA.Models;
using BERKA.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BERKA.Web.Controllers
{
    public class InspectorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public InspectorController(IHttpClientFactory factory, IConfiguration config)
        {
            _httpClient = factory.CreateClient();
            _apiBaseUrl = config["ApiSettings:BaseUrl"];
        }

        
        [HttpGet]
        public IActionResult RevisarVehiculo(int idCita)
        {
            var model = new RevisionViewModel
            {
                ID_Cita = idCita
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarRevision(RevisionViewModel model)
        {
            var resultadoFinal = (model.Luces == "Aprobado" && model.Suspension == "Aprobado" &&
                                  model.Frenos == "Aprobado" && model.Neumaticos == "Aprobado" &&
                                  model.Carroceria == "Aprobado" && model.Gases == "Aprobado")
                                  ? "Aprobado" : "Reprobado";

            var revision = new
            {
                ID_Cita = model.ID_Cita,
                ID_Vehiculo = model.ID_Vehiculo,
                Resultado = resultadoFinal,
                Observaciones = model.Observaciones ?? "",
                FechaRevision = DateTime.Now
            };

            var json = JsonConvert.SerializeObject(revision);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/revision", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "¡Revisión registrada correctamente!";
                return RedirectToAction("RevisarVehiculo");
            }

            var error = await response.Content.ReadAsStringAsync();
            ViewBag.Error = $"Error al registrar: {response.StatusCode} - {error}";
            return View("RevisarVehiculo");
        }

        // === Agregar método para eliminar vehículo ===
        [HttpPost]
        public async Task<IActionResult> EliminarVehiculo(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/vehiculo/{id}");
            return RedirectToAction("Dashboard");
        }

        public IActionResult Revision()
        {
            return View(new RevisionViewModel());
        }
    }
}