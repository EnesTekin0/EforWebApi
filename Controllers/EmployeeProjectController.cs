using Microsoft.AspNetCore.Mvc;
using EforWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EforWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeProjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeProject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeProject>>> GetEmployeeProjects()
        {
            return await _context.EmployeeProjects.ToListAsync();
        }

        // GET: api/EmployeeProject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeProject>> GetEmployeeProject(int id)
        {
            var employeeProject = await _context.EmployeeProjects.FindAsync(id);

            if (employeeProject == null)
            {
                return NotFound();
            }

            return employeeProject;
        }

        // POST: api/EmployeeProject
        [HttpPost]
        public async Task<ActionResult<EmployeeProject>> PostEmployeeProject(EmployeeProject employeeProject)
        {
            _context.EmployeeProjects.Add(employeeProject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeProject", new { id = employeeProject.EmployeeProjectId }, employeeProject);
        }

        // PUT: api/EmployeeProject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeProject(int id, EmployeeProject employeeProject)
        {
            if (id != employeeProject.EmployeeProjectId)
            {
                return BadRequest();
            }

            _context.Entry(employeeProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeProjectExists(id))
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

        // DELETE: api/EmployeeProject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeProject(int id)
        {
            var employeeProject = await _context.EmployeeProjects.FindAsync(id);
            if (employeeProject == null)
            {
                return NotFound();
            }

            _context.EmployeeProjects.Remove(employeeProject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeProjectExists(int id)
        {
            return _context.EmployeeProjects.Any(e => e.EmployeeProjectId == id);
        }
    }
}
