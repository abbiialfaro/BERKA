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

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.InspectoresActivos = await _context.Inspectores.CountAsync(i => i.Estado == "Activo");
            ViewBag.TotalCitas = await _context.Citas.CountAsync();
            ViewBag.Aprobados = await _context.Revisiones.CountAsync(r => r.Resultado == "Aprobado");
            ViewBag.Rechazados = await _context.Revisiones.CountAsync(r => r.Resultado == "Rechazado");

            var clientes = await _context.Clientes.ToListAsync();
            var citas = await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo)
                .Include(c => c.Inspector)
                .ToListAsync();

            var revisiones = await _context.Revisiones
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Cliente)
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Vehiculo)
                .ToListAsync();

            var vehiculos = await _context.Vehiculos.ToListAsync();

            ViewBag.Clientes = clientes;
            ViewBag.Citas = citas;
            ViewBag.Revisiones = revisiones;
            ViewBag.Vehiculos = vehiculos;

            return View("Index");
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


