using BERKA.Models;
using Microsoft.AspNetCore.Mvc;

public class CitaController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public CitaController(IHttpClientFactory factory, IConfiguration config)
    {
        _httpClient = factory.CreateClient();
        _apiBaseUrl = config["ApiSettings:BaseUrl"];
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/cita");
        if (response.IsSuccessStatusCode)
        {
            var citas = await response.Content.ReadFromJsonAsync<List<Cita>>();
            return View(citas); // requiere vista Razor Index.cshtml
        }

        ViewBag.Error = "Error al cargar citas.";
        return View(new List<Cita>());
    }
}