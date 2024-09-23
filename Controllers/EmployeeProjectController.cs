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
    public class EmployeeProjectControllerr : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeProjectControllerr(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeProject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeProject>>> GetEmployeeProjects()
        {
            if (_context.EmployeeProjects == null)
            {
                return NotFound();
            }
            return await _context.EmployeeProjects.ToListAsync();
        }

        // GET: api/EmployeeProject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeProject>> GetEmployeeProject(int id)
        {
            if (_context.EmployeeProjects == null)
            {
                return NotFound();
            }
            var employeeProject = await _context.EmployeeProjects.FindAsync(id);

            if (employeeProject == null)
            {
                return NotFound();
            }

            return employeeProject;
        }

        // POST: api/EmployeeProject
        [HttpPost]
        public async Task<ActionResult<EmployeeProject>> PostEmployeeProject(EmployeeProjectDto employeeProjectDto)
        {
            if (employeeProjectDto == null)
            {
                return BadRequest("Employee data is null.");
            }
            var result = _context.EmployeeProjects.Add(new EmployeeProject
            {
                EmployeeId = employeeProjectDto.EmployeeId,
                ProjectId = employeeProjectDto.ProjectId,
                EffortGoals = employeeProjectDto.EffortGoals,
                EffortAmount = employeeProjectDto.EffortAmount,
                StartDate = employeeProjectDto.StartDate,
                EndDate = employeeProjectDto.EndDate
            });
            await _context.SaveChangesAsync();
            return Ok(result.Entity);
        }

        // PUT: api/EmployeeProject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeProject(int id, EmployeeProjectDto employeeProjectDto)
        {
            var employeeProject = await _context.EmployeeProjects.FindAsync(id);

            if (employeeProject == null)
            {
                return NotFound();
            }

            employeeProject.EmployeeId = employeeProjectDto.EmployeeId;
            employeeProject.ProjectId = employeeProjectDto.ProjectId;
            employeeProject.EffortGoals = employeeProjectDto.EffortGoals;
            employeeProject.EffortAmount = employeeProjectDto.EffortAmount;
            employeeProject.StartDate = employeeProjectDto.StartDate;
            employeeProject.EndDate = employeeProjectDto.EndDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EmployeeProjects.Any(ep => ep.EmployeeProjectId == id))
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