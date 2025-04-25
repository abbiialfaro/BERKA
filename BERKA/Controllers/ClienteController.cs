using BERKA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BERKA.Share.ViewModels;

namespace BERKA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly BERKAcontext _context;

        public ClienteController(BERKAcontext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return NotFound();
            return c;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClienteViewModel model)
        {
            Console.WriteLine("👉 API recibió POST con Nombre=" + model.Nombre);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = new Cliente
            {
                Tipo_Documento = model.TipoDocumento,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Correo = model.Correo,
                Telefono = model.Telefono,
                Direccion = model.Direccion,
                Categoria = model.Categoria
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Opcional: devolver CreatedAtAction para REST completo
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ID_Cliente }, cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Opcional: log de errores
                foreach (var e in ModelState.Values.SelectMany(v => v.Errors))
                    Console.WriteLine("Error de validación: " + e.ErrorMessage);
                return BadRequest(ModelState);
            }

            // Busca la entidad existente
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            // Mapea los campos del ViewModel a la entidad
            cliente.Tipo_Documento = model.TipoDocumento;
            cliente.Nombre = model.Nombre;
            cliente.Apellido = model.Apellido;
            cliente.Correo = model.Correo;
            cliente.Telefono = model.Telefono;
            cliente.Direccion = model.Direccion;
            cliente.Categoria = model.Categoria;

            await _context.SaveChangesAsync();
            return NoContent();  // 204
        }
    }
}