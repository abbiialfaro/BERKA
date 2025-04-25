using BERKA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tipos_PlacaController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public Tipos_PlacaController(BERKAcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipos_Placa>>> GetTipos_Placa()
        {
            return await _context.Tipos_Placas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tipos_Placa>> GetTipos_Placa(int id)
        {
            var tipos_Placa = await _context.Tipos_Placas.FindAsync(id);

            if (tipos_Placa == null)
            {
                return NotFound();
            }

            return tipos_Placa;
        }

        [HttpPost]
        public async Task<ActionResult<Tipos_Placa>> PostTipos_Placa(Tipos_Placa tipos_Placa)
        {
            _context.Tipos_Placas.Add(tipos_Placa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipos_Placa", new { id = tipos_Placa.ID_Tip_Placa }, tipos_Placa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipos_Placa(int id)
        {
            var tipos_Placa = await _context.Tipos_Placas.FindAsync(id);
            if (tipos_Placa == null)
            {
                return NotFound();
            }

            _context.Tipos_Placas.Remove(tipos_Placa);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipos_Placa(int id, Tipos_Placa tipos_Placa)
        {
            if (id != tipos_Placa.ID_Tip_Placa)
            {
                return BadRequest();
            }

            _context.Entry(tipos_Placa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tipos_Placas.Any(e => e.ID_Tip_Placa == id))
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


