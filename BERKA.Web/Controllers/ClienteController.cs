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

        // Dashboard Cliente
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clientes = await _http.GetFromJsonAsync<List<Cliente>>("cliente");
            return View(clientes);
        }

        // Registrar Cliente
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

        // GET: /Cliente/EditarCliente/5
        [HttpGet]
        public async Task<IActionResult> EditarCliente(int id)
        {
            // 1. Trae la entidad desde el API
            var clienteEntity = await _http.GetFromJsonAsync<Cliente>($"cliente/{id}");
            if (clienteEntity == null)
                return NotFound();

            // 2. Mapea a tu ClienteViewModel
            var model = new ClienteViewModel
            {
                ID_Cliente = clienteEntity.ID_Cliente,
                TipoDocumento = clienteEntity.Tipo_Documento,
                Nombre = clienteEntity.Nombre,
                Apellido = clienteEntity.Apellido,
                Correo = clienteEntity.Correo,
                Telefono = clienteEntity.Telefono,
                Direccion = clienteEntity.Direccion,
                Categoria = clienteEntity.Categoria
            };

            // 3. Devuelve el ViewModel a la vista
            return View(model);
        }


        // POST: /Cliente/EditarCliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCliente(ClienteViewModel model)
        {
            Console.WriteLine($"👉 POST EditarCliente MVC, ID={model.ID_Cliente}, Nombre={model.Nombre}");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _http.PutAsJsonAsync($"cliente/{model.ID_Cliente}", model);
                Console.WriteLine("👉 Respuesta API PUT: " + response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("", $"Error API: {response.StatusCode}");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Excepción en PUT: " + ex.GetBaseException().Message);
                ModelState.AddModelError("", "No se pudo conectar al API.");
                return View(model);
            }

            TempData["mensaje"] = "¡Cliente actualizado con éxito!";
            return RedirectToAction(nameof(Index));
        }

        // /Cliente/EditarCliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            Console.WriteLine($"👉 POST EliminarCliente MVC, ID={id}");
            var response = await _http.DeleteAsync($"cliente/{id}");
            Console.WriteLine("👉 Respuesta API DELETE: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
                TempData["mensaje"] = "¡Cliente eliminado exitosamente!";
            else
                TempData["mensaje"] = "Error al eliminar el cliente.";

            return RedirectToAction(nameof(Index));
        }


    }
}
