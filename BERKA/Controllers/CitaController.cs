using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BERKA.Models;
using CitaApi = BERKA.Models.Cita;  

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public CitaController(BERKAcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CitaApi>>> GetCita()
        {
            return await _context.Citas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> PostCita(CitaRequest request)
        {
            if (string.IsNullOrEmpty(request.Placa))
            {
                return BadRequest("La placa del vehículo es obligatoria.");
            }

            var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Placa == request.Placa);
            if (vehiculo == null)
                return BadRequest("No se encontró un vehículo con esa placa.");

            var inspector = await _context.Inspectores.FirstOrDefaultAsync(i => i.Estado == "Activo");
            if (inspector == null)
                return BadRequest("No hay inspectores disponibles.");

            var cita = new Cita
            {
                ID_Cliente = request.ID_Cliente,
                Fecha = request.Fecha,
                Hora = request.Hora,
                Estado = request.Estado,
                ID_Vehiculo = vehiculo.ID_Vehiculo,
                ID_Inspector = inspector.ID_Inspector
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCita), new { id = cita.ID_Cita }, cita);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                return NotFound();

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, CitaApi cita)
        {
            if (id != cita.ID_Cita)
                return BadRequest();

            _context.Entry(cita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Citas.Any(e => e.ID_Cita == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }
    }
}