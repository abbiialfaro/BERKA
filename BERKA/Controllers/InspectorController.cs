using BERKA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectorController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public InspectorController(BERKAcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inspector>>> GetInspector()
        {
            return await _context.Inspectores.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inspector>> GetInspector(int id)
        {
            var inspector = await _context.Inspectores.FindAsync(id);

            if (inspector == null)
            {
                return NotFound();
            }

            return inspector;
        }

        [HttpPost]
        public async Task<ActionResult<Inspector>> PostInspector(Inspector inspector)
        {
            _context.Inspectores.Add(inspector);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInspector", new { id = inspector.ID_Inspector }, inspector);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspector(int id)
        {
            var inspector = await _context.Inspectores.FindAsync(id);
            if (inspector == null)
            {
                return NotFound();
            }

            _context.Inspectores.Remove(inspector);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInspector(int id, Inspector inspector)
        {
            if (id != inspector.ID_Inspector)
            {
                return BadRequest();
            }

            _context.Entry(inspector).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Inspectores.Any(e => e.ID_Inspector == id))
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

