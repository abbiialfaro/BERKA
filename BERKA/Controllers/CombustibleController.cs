using BERKA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombustibleController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public CombustibleController (BERKAcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Combustible>>> GetCombustible()
        {
            return await _context.Combustibles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Combustible>> GetCombustible(int id)
        {
            var combustible = await _context.Combustibles.FindAsync(id);

            if (combustible == null)
            {
                return NotFound();
            }

            return combustible;
        }

        [HttpPost]
        public async Task<ActionResult<Combustible>> PostCombustible(Combustible combustible)
        {
            _context.Combustibles.Add(combustible);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCombustible", new { id = combustible.ID_Tip_Comb }, combustible);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCombustible(int id)
        {
            var combustible = await _context.Combustibles.FindAsync(id);
            if (combustible == null)
            {
                return NotFound();
            }

            _context.Combustibles.Remove(combustible);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCombustible(int id, Combustible combustible)
        {
            if (id != combustible.ID_Tip_Comb)
            {
                return BadRequest();
            }

            _context.Entry(combustible).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Combustibles.Any(e => e.ID_Tip_Comb == id))
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
