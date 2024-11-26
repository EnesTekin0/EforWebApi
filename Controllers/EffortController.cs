using Microsoft.AspNetCore.Mvc;
using EforWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using EforWebApi.DTO;

namespace EforWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EffortController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EffortController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Effort
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Effort>>> GetEfforts()
        {
            if (_context.Efforts == null)
            {
                return NotFound();
            }
            return await _context.Efforts.ToListAsync();
        }

        // GET: api/Effort/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Effort>> GetEffort(int id)
        {
            if (_context.Efforts == null)
            {
                return NotFound();
            }
            var effort = await _context.Efforts.FindAsync(id);

            if (effort == null)
            {
                return NotFound();
            }

            return effort;
        }

        // POST: api/Effort
        [HttpPost]
        public async Task<ActionResult<Effort>> PostEffort(EffortDto effortDto)
        {
            if (effortDto == null)
            {
                return BadRequest("Employee data is null.");
            }
            var result = _context.Efforts.Add(new Effort
            {
                EffortAmount = effortDto.EffortAmount,
                EffortDate = effortDto.EffortDate,
                EmployeeProjectId = effortDto.EmployeeProjectId
            });
            await _context.SaveChangesAsync();
            return Ok(result.Entity);
        }

        // PUT: api/Effort/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEffort(int id, EffortDto effortDto)
        {
            var effort = await _context.Efforts.FindAsync(id);

            if (effort == null)
            {
                return NotFound();
            }

            effort.EffortAmount = effortDto.EffortAmount;
            effort.EffortDate = effortDto.EffortDate;
            effort.EmployeeProjectId = effortDto.EmployeeProjectId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Efforts.Any(e => e.EffortId == id))
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