using BERKA.Share.ViewModels; // o el namespace correcto
using BERKA.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BERKA.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly HttpClient _http;

        public ClienteController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiCliente");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
            return View(clientes);
        }

        [HttpGet]
        public IActionResult RegistrarCliente()
        {
            Console.WriteLine("Entró al GET de RegistrarCliente");
            return View();  // busca /Views/Cliente/RegistrarCliente.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarCliente(ClienteViewModel model)
        {
            Console.WriteLine("👉 Entré al POST MVC con Nombre=" + model.Nombre);

            try
            {
                var response = await _http.PostAsJsonAsync("cliente", model);
                Console.WriteLine("👉 Respuesta API: " + response.StatusCode);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Código: " + response.StatusCode);

                TempData["mensaje"] = "¡Cliente registrado con éxito!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al llamar API: " + ex.GetBaseException().Message);
                ModelState.AddModelError("", "¡Uy! No pude comunicarme con el API: " + ex.Message);
                return View(model);
            }
        }



        // similar para EditarCliente…
    }
}
