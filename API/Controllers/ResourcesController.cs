using API.Data;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")] // Adres to: /api/resources
    [ApiController]
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Wstrzykiwanie bazy danych przez konstruktor (Dependency Injection)
        public ResourcesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/resources
        // Pobiera wszystkie zasoby
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            return await _context.Resources.ToListAsync();
        }

        // 2. GET: api/resources/5
        // Pobiera jeden zasób po ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(Guid id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound(); // Zwraca kod 404
            }

            return resource; // Zwraca kod 200 + obiekt
        }

        // 3. POST: api/resources
        // Tworzy nowy zasób
        [HttpPost]
        public async Task<ActionResult<Resource>> CreateResource(Resource resource)
        {
            // Nadpisujemy ID na nowe (na wypadek gdyby ktoś przysłał własne)
            resource.Id = Guid.NewGuid();
            resource.CreatedAt = DateTime.UtcNow;

            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            // Zwracamy kod 201 Created oraz lokalizację nowego obiektu
            return CreatedAtAction("GetResource", new { id = resource.Id }, resource);
        }

        // 4. PUT: api/resources/5
        // Aktualizuje istniejący zasób
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResource(Guid id, Resource resource)
        {
            if (id != resource.Id)
            {
                return BadRequest();
            }

            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Resources.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Kod 204 (Sukces, brak treści do zwrócenia)
        }

        // 5. DELETE: api/resources/5
        // Usuwa zasób
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(Guid id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}