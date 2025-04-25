using BERKA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstacionController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public EstacionController(BERKAcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estacion>>> GetEstacion()
        {
            return await _context.Estaciones.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estacion>> GetEstacion(int id)
        {
            var estacion = await _context.Estaciones.FindAsync(id);

            if (estacion == null)
            {
                return NotFound();
            }

            return estacion;
        }

        [HttpPost]
        public async Task<ActionResult<Estacion>> PostEstacion(Estacion estacion)
        {
            _context.Estaciones.Add(estacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstacion", new { id = estacion.ID_Estacion }, estacion);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstacion(int id)
        {
            var estacion = await _context.Estaciones.FindAsync(id);
            if (estacion == null)
            {
                return NotFound();
            }

            _context.Estaciones.Remove(estacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstacion(int id, Estacion estacion)
        {
            if (id != estacion.ID_Estacion)
            {
                return BadRequest();
            }

            _context.Entry(estacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Estaciones.Any(e => e.ID_Estacion == id))
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

