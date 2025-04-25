using BERKA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Detalle_RevisionController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public Detalle_RevisionController(BERKAcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Detalle_Revision>>> GetDetalle_Revision()
        {
            return await _context.Detalle_Revisiones.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Detalle_Revision>> GetDetalle_Revision(int id)
        {
            var detalle_Revision = await _context.Detalle_Revisiones.FindAsync(id);

            if (detalle_Revision == null)
            {
                return NotFound();
            }

            return detalle_Revision;
        }

        [HttpPost]
        public async Task<ActionResult<Detalle_Revision>> PostDetalle_Revision(Detalle_Revision detalle_Revision)
        {
            _context.Detalle_Revisiones.Add(detalle_Revision);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalle_Revision", new { id = detalle_Revision.ID_Detalle }, detalle_Revision);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalle_Revision(int id)
        {
            var detalle_Revision = await _context.Detalle_Revisiones.FindAsync(id);
            if (detalle_Revision == null)
            {
                return NotFound();
            }

            _context.Detalle_Revisiones.Remove(detalle_Revision);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalle_Revision(int id, Detalle_Revision detalle_Revision)
        {
            if (id != detalle_Revision.ID_Detalle)
            {
                return BadRequest();
            }

            _context.Entry(detalle_Revision).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Detalle_Revisiones.Any(e => e.ID_Detalle == id))
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

    