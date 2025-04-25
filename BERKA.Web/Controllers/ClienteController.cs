// Esta clase manejara las acciones al darle al boton submit de la vista de administrar clientes

using BERKA.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class ClientesController : Controller
{
    private readonly HttpClient _http;

    public ClientesController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApiCliente");
    }

    [HttpGet]
    public IActionResult RegistrarCliente()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrarCliente(ClienteViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var response = await _http.PostAsJsonAsync("clientes", model);

        if (response.IsSuccessStatusCode)
        {
            TempData["Éxito"] = "¡Cliente registrado correctamente!";
            return RedirectToAction("Index"); // o a donde lo necesites
        }

        ModelState.AddModelError(string.Empty, "Hubo un error al guardar el cliente.");
        return View(model);
    }
}
