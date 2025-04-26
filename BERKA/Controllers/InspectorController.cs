using BERKA.Models;
using BERKA.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class InspectorController : ControllerBase
{
    private readonly BERKAcontext _context;
    public InspectorController(BERKAcontext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Inspector>>> Get()
        => await _context.Inspectores.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Inspector>> Get(int id)
    {
        var i = await _context.Inspectores.FindAsync(id);
        return i == null ? NotFound() : Ok(i);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InspectorViewModel m)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var i = new Inspector
        {
            Nombre = m.Nombre,
            Apellido = m.Apellido,
            Correo = m.Correo,
            Telefono = m.Telefono,
            Estacion = m.Estacion,
            Contraseña = m.Contraseña,
            Estado = m.Estado
        };
        _context.Inspectores.Add(i);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = i.ID_Inspector }, i);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] InspectorViewModel m)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var i = await _context.Inspectores.FindAsync(id);
        if (i == null) return NotFound();
        i.Nombre = m.Nombre;
        i.Apellido = m.Apellido;
        i.Correo = m.Correo;
        i.Telefono = m.Telefono;
        i.Estacion = m.Estacion;
        i.Contraseña = m.Contraseña;
        i.Estado = m.Estado;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var i = await _context.Inspectores.FindAsync(id);
        if (i == null) return NotFound();
        _context.Inspectores.Remove(i);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
