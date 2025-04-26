using BERKA.Models;
using BERKA.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CitaController : ControllerBase
{
    private readonly BERKAcontext _context;
    public CitaController(BERKAcontext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
    {
        return await _context.Citas
                             .Include(c => c.Cliente)
                             .Include(c => c.Vehiculo)
                             .Include(c => c.Inspector)
                             .ToListAsync();
    }

    // POST: api/cita
    [HttpPost]
    public async Task<IActionResult> RegistrarCita([FromBody] CitaViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cita = new Cita
        {
            Fecha = model.Fecha,
            Hora = model.Hora,
            Estado = model.Estado,
            ID_Cliente = model.ID_Cliente,
            ID_Vehiculo = model.ID_Vehiculo,
            ID_Inspector = model.ID_Inspector
        };

        _context.Citas.Add(cita);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCita), new { id = cita.ID_Cita }, cita);
    }

    // PUT: api/cita/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CitaViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cita = await _context.Citas.FindAsync(id);
        if (cita == null) return NotFound();

        cita.Fecha = model.Fecha;
        cita.Hora = model.Hora;
        cita.Estado = model.Estado;
        cita.ID_Cliente = model.ID_Cliente;
        cita.ID_Vehiculo = model.ID_Vehiculo;
        cita.ID_Inspector = model.ID_Inspector;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET api/cita/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Cita>> GetCita(int id)
    {
        var c = await _context.Citas.FindAsync(id);
        return c is null ? NotFound() : Ok(c);
    }

    // DELETE api/cita/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCita(int id)
    {
        var cita = await _context.Citas.FindAsync(id);
        if (cita == null) return NotFound();
        _context.Citas.Remove(cita);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
