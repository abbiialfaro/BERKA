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
        public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculo()
        {
            return await _context.Vehiculos.ToListAsync();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehiculo(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.ID_Vehiculo)
            {
                return BadRequest();
            }

            _context.Entry(vehiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Vehiculos.Any(e => e.ID_Vehiculo == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}


