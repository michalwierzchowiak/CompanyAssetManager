using API.Data;
using Core;
using Core.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResourcesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        public ResourcesController(AppDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
        {
            return await _context.Resources.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(Guid id)
        {
            var resource = await _context.Resources.FindAsync(id);

            if (resource == null)
            {
                return NotFound(); 
            }

            return resource;
        }

        [HttpPost]
        public async Task<ActionResult<Resource>> CreateResource(Resource resource)
        {
            resource.Id = Guid.NewGuid();
            resource.CreatedAt = DateTime.UtcNow;

            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish(new ResourceCreated
            {
                Id = resource.Id,
                Name = resource.Name,
                Description = resource.Description,
                CreatedAt = resource.CreatedAt
            });

            return CreatedAtAction("GetResource", new { id = resource.Id }, resource);
        }

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

            return NoContent();
        }

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