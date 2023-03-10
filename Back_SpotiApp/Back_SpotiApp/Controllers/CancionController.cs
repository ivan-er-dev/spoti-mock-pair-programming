using Back_SpotiApp.Config;
using Back_SpotiApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_SpotiApp.Controllers
{
    [Route("/api/cancion")]
    public class CancionController : Controller
    {
        private readonly DBSpotiContext _context;

        public CancionController(DBSpotiContext context)
        {
            _context = context;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<Cancion>>> Get()
        {
            return await _context.Canciones.Include(x=>x.Genero).ToListAsync();
        }

        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody] Cancion cancion)
        {
            

                _context.Add(cancion);
                await _context.SaveChangesAsync();
                return Ok(cancion);

        }

        [HttpGet("buscar-id/{id}")]
        public async Task<ActionResult<Cancion>> GetCancion(int id)
        {
            var cancionExist = await _context.Canciones.AnyAsync(x => x.Id == id);
            if (!cancionExist)
            {
                return BadRequest($"La cancion con id {id} no existe");
            }
            return await _context.Canciones.Include(x=>x.Genero).FirstOrDefaultAsync(x=>x.Id == id);
        }

        [HttpGet("buscar-nombre/{name}")]
        public async Task<ActionResult<Cancion>> GetCancion(String name)
        {
            var cancionExist = await _context.Canciones.AnyAsync(x => x.Nombre == name);
            if (!cancionExist)
            {
                return BadRequest($"La cancion con nombre {name} no existe");
            }
            return await _context.Canciones.Include(x => x.Genero).FirstOrDefaultAsync(x => x.Nombre == name);
        }

        [HttpPut("/update/{id}")]
        public async Task<ActionResult> Put(int id, Cancion cancion)
        {
            _context.Entry(cancion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(cancion);
        }

        [HttpDelete("/delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cancion = await _context.Canciones.FindAsync(id);
            _context.Remove(cancion);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
