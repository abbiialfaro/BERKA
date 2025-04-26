using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BERKA.Models;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BERKA.Web.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly BERKAcontext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public AdministradorController(BERKAcontext context, IHttpClientFactory factory, IConfiguration config)
        {
            _context = context;
            _httpClient = factory.CreateClient();
            _apiBaseUrl = config["ApiSettings:BaseUrl"];
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var today = DateTime.Today;

            // 1) Generar las 7 fechas: hoy y los 6 días anteriores
            var fechas = Enumerable
                .Range(0, 7)                                // 0,1,2,3,4,5,6
                .Select(i => today.AddDays(i - 6))          // -6,-5,...,0 → fechas de hace 6 días hasta hoy
                .ToList();

            // 2) Contar citas por cada fecha
            var citasCounts = new List<int>();
            foreach (var fecha in fechas)
            {
                var count = await _context.Citas
                    .CountAsync(c => c.Fecha.Date == fecha);
                citasCounts.Add(count);
            }

            // 3) Contar revisiones
            var aprobados = await _context.Revisiones
                .CountAsync(r => r.Resultado == "Aprobado");
            var rechazados = await _context.Revisiones
                .CountAsync(r => r.Resultado == "Rechazado");

            // 4) Pasar todo a la vista
            ViewBag.CitasLabels = fechas
                .Select(f => f.ToString("dd-MMM"))
                .ToList();
            ViewBag.CitasData = citasCounts;
            ViewBag.Aprobados = aprobados;
            ViewBag.Rechazados = rechazados;

            ViewBag.Vehiculos = await _context.Vehiculos.Include(v => v.Cliente).ToListAsync();
            ViewBag.Citas = await _context.Citas
                                    .Include(c => c.Cliente)
                                    .Include(c => c.Vehiculo)
                                    .Include(c => c.Inspector)
                                    .ToListAsync();
            ViewBag.Revisiones = await _context.Revisiones
                                        .Include(r => r.Cita)
                                        .ThenInclude(c => c.Cliente)
                                        .Include(r => r.Cita)
                                        .ThenInclude(c => c.Vehiculo)
                                        .ToListAsync();
            ViewBag.Clientes = await _context.Clientes.ToListAsync();

            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> EliminarVehiculo(int id)
        {
            await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/vehiculo/{id}");
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/cliente/{id}");
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCita(int id)
        {
            await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/cita/{id}");
            return RedirectToAction("Dashboard");
        }

        // Vistas de los paneles de Administracion
        public IActionResult Clientes()
        {
            return View("AdministradorClientes");
        }

        public IActionResult Citas()
        {
            ViewBag.Clientes = _context.Clientes.ToList();
            ViewBag.Vehiculos = _context.Vehiculos.ToList();
            ViewBag.Citas = _context.Citas.Include(c => c.Cliente).Include(c => c.Vehiculo).ToList();
            ViewBag.Revisiones = _context.Revisiones.ToList();

            return View("AdministradorCitas");
        }

        public IActionResult Combustibles()
        {
            return View("AdministradorCombustibles");
        }

        public IActionResult Estaciones()
        {
            return View("AdministradorEstaciones");
        }

        public IActionResult Revisiones()
        {
            return View("AdministradorRevisiones");
        }

        public IActionResult Vehiculos()
        {
            return View("AdministradorVehiculos");
        }
    }
}


