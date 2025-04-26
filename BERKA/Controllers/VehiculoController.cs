using BERKA.Models;
using BERKA.Share.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public VehiculoController(BERKAcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculos()
        {
            var vehiculosConCliente = await _context.Vehiculos
                .Include(v => v.Cliente)      // <-- incluye la FK
                .ToListAsync();

            return Ok(vehiculosConCliente);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculo(int id)
        {
            var v = await _context.Vehiculos.FindAsync(id);
            if (v == null) return NotFound();
            return v;
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
        {
            var categorias = await _context.Vehiculos
                .Select(v => v.Categoria)
                .Distinct()
                .ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("porplaca/{placa}")]
        public async Task<ActionResult<Vehiculo>> GetVehiculoPorPlaca(string placa)
        {
            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Placa == placa);

            if (vehiculo == null)
            {
                return NotFound();
            }

            return vehiculo;
        }




        [HttpGet("buscar")]
        public async Task<ActionResult<Vehiculo>> BuscarPorPlacaYCategoria([FromQuery] string placa, [FromQuery] string categoria)
        {
            var vehiculo = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Placa.ToUpper() == placa.ToUpper());

            if (vehiculo == null)
            {
                return NotFound();
            }

            return vehiculo;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VehiculoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var veh = new Vehiculo
            {
                Marca = model.Marca,
                Modelo = model.Modelo,
                Categoria = model.Categoria,
                Color = model.Color,
                Año = model.Año,
                Placa = model.Placa,
                tip_Combustible = model.Tip_Combustible,
                Kilometraje = model.Kilometraje,
                ID_Cliente = model.ID_Cliente
            };

            _context.Vehiculos.Add(veh);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehiculo), new { id = veh.ID_Vehiculo }, veh);
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/vehiculo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] VehiculoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var veh = await _context.Vehiculos.FindAsync(id);
            if (veh == null) return NotFound();

            // Mapea los campos actualizados
            veh.Marca = model.Marca;
            veh.Modelo = model.Modelo;
            veh.Categoria = model.Categoria;
            veh.Color = model.Color;
            veh.Año = model.Año;
            veh.Placa = model.Placa;
            veh.tip_Combustible = model.Tip_Combustible;
            veh.Kilometraje = model.Kilometraje;
            veh.ID_Cliente = model.ID_Cliente;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}


