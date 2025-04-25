using BERKA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisionController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public RevisionController(BERKAcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Revision>>> GetRevision()
        {
            return await _context.Revisiones.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Revision>> GetRevision(int id)
        {
            var revision = await _context.Revisiones.FindAsync(id);
            if (revision == null) return NotFound();
            return revision;
        }

        [HttpGet("porvehiculo/{idVehiculo}")]
        public async Task<ActionResult<Revision>> GetRevisionPorVehiculo(int idVehiculo)
        {
            var revision = await _context.Revisiones.FirstOrDefaultAsync(r => r.ID_Vehiculo == idVehiculo);
            if (revision == null) return NotFound();
            return revision;
        }
        [HttpPost]
        public async Task<IActionResult> PostRevision([FromBody] Revision revision)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Revisiones.Add(revision);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRevision", new { id = revision.ID_Rev }, revision);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutRevision(int id, Revision revision)
        {
            if (id != revision.ID_Rev)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(revision).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Revisiones.Any(e => e.ID_Rev == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRevision(int id)
        {
            var revision = await _context.Revisiones.FindAsync(id);
            if (revision == null) return NotFound();

            _context.Revisiones.Remove(revision);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}



