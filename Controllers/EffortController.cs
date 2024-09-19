using Microsoft.AspNetCore.Mvc;
using EforWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EforWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EffortControllerr : ControllerBase
    {
        private readonly AppDbContext _context;

        public EffortControllerr(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Effort
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Effort>>> GetEfforts()
        {
            return await _context.Efforts.ToListAsync();
        }

        // GET: api/Effort/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Effort>> GetEffort(int id)
        {
            var effort = await _context.Efforts.FindAsync(id);

            if (effort == null)
            {
                return NotFound();
            }

            return effort;
        }

        // POST: api/Effort
        [HttpPost]
        public async Task<ActionResult<Effort>> PostEffort(Effort effort) //DTO
        {
            _context.Efforts.Add(effort);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEffort", new { id = effort.EffortId }, effort);
        }

        // PUT: api/Effort/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEffort(int id, Effort effort)
        {
            if (id != effort.EffortId)
            {
                return BadRequest();
            }

            _context.Entry(effort).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EffortExists(id))
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

        // DELETE: api/Effort/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEffort(int id)
        {
            var effort = await _context.Efforts.FindAsync(id);
            if (effort == null)
            {
                return NotFound();
            }

            _context.Efforts.Remove(effort);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EffortExists(int id)
        {
            return _context.Efforts.Any(e => e.EffortId == id);
        }
    }
}
