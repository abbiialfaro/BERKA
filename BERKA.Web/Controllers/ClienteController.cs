using BERKA.Models;
using BERKA.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult Registrar() => View();

    [HttpPost]
    public async Task<IActionResult> Registrar(ClienteViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var response = await _http.PostAsJsonAsync("cliente", model);
        return RedirectToAction("Index");
    }
}
