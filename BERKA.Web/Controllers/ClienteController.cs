using BERKA.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BERKA.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ClienteController(IHttpClientFactory factory, IConfiguration config)
        {
            _httpClient = factory.CreateClient();
            _apiBaseUrl = config["ApiSettings:BaseUrl"];
        }

        [HttpGet]
        public async Task<IActionResult> SacarCita()
        {
            var model = new CitaViewModel
            {
                Fecha = DateTime.Today,
                Hora = TimeSpan.FromHours(9)
            };

            await CargarCategorias(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SacarCita(CitaViewModel model)
        {
            var citaRequest = new CitaRequest
            {
                ID_Cliente = 1,
                Fecha = model.Fecha,
                Hora = model.Hora,
                Estado = "Pendiente",
                Placa = model.Placa,
                Categoria = model.Categoria
            };

            var json = JsonConvert.SerializeObject(citaRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/cita", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "¡Cita agendada correctamente!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ViewBag.Mensaje = $"Error al agendar la cita. Código: {response.StatusCode} - {error}";
            }

            await CargarCategorias(model);
            return View(model);
        }

        private async Task CargarCategorias(CitaViewModel model)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/vehiculo/categorias");
            if (response.IsSuccessStatusCode)
            {
                var categorias = await response.Content.ReadFromJsonAsync<List<string>>();
                model.Categorias = categorias ?? new();
            }
        }
    }
}