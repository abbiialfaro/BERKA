using BERKA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tipos_VehiculoController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public Tipos_VehiculoController (BERKAcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipos_Vehiculo>>> GetTipos_Vehiculo()
        {
            return await _context.Tipos_Vehiculos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tipos_Vehiculo>> GetTipos_Vehiculo(int id)
        {
            var tipos_Vehiculo = await _context.Tipos_Vehiculos.FindAsync(id);

            if (tipos_Vehiculo == null)
            {
                return NotFound();
            }

            return tipos_Vehiculo;
        }

        [HttpPost]
        public async Task<ActionResult<Tipos_Vehiculo>> PostTipos_Vehiculo(Tipos_Vehiculo tipos_Vehiculo)
        {
            _context.Tipos_Vehiculos.Add(tipos_Vehiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("´GetTipos_Vehiculo", new { id = tipos_Vehiculo.ID_Tipo_Vehiculo }, tipos_Vehiculo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipos_Vehiculo(int id)
        {
            var tipos_Vehiculo = await _context.Tipos_Vehiculos.FindAsync(id);
            if (tipos_Vehiculo == null)
            {
                return NotFound();
            }

            _context.Tipos_Vehiculos.Remove(tipos_Vehiculo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipos_Vehiculo(int id, Tipos_Vehiculo tipos_Vehiculo)
        {
            if (id != tipos_Vehiculo.ID_Tipo_Vehiculo)
            {
                return BadRequest();
            }

            _context.Entry(tipos_Vehiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tipos_Vehiculos.Any(e => e.ID_Tipo_Vehiculo == id))
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


