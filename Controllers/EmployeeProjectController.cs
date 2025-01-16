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


            var epEffort = await _context.EmployeeProjects.FirstOrDefaultAsync();
            if (epEffort != null)
            {
                _ = epEffort.Effort.ToList(); //NEW
            }

            return employeeProject;
        }


[HttpGet("employee-report/{employeeId}/{year}")]
public async Task<ActionResult<EmployeeProjectReportDto>> GetEmployeeProjectReport(int employeeId, int year)
{
    // Çalışan bilgilerini al
    var employee = await _context.Employees
        .Where(e => e.EmployeeId == employeeId)
        .FirstOrDefaultAsync();

    if (employee == null)
    {
        return NotFound("Çalışan bulunamadı.");
    }

    // Çalışanın projelerini ve effort bilgilerini al
    var projectEfforts = await _context.EmployeeProjects
        .Include(ep => ep.Project)
        .Include(ep => ep.Effort)
        .Where(ep => ep.EmployeeId == employeeId)
        .Select(ep => new ProjectEffortDetail
        {
            ProjectName = ep.Project.ProjectName,
            MonthlyEfforts = ep.Effort
                .Where(e => e.EffortDate.Year == year)
                .GroupBy(e => e.EffortDate.Month)
                .Select(g => new MonthlyEffort
                {
                    Month = g.Key,
                    EffortAmount = g.Sum(e => e.EffortAmount)
                })
                .ToList()
        })
        .ToListAsync();

    var report = new EmployeeProjectReportDto
    {
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Projects = projectEfforts
    };

    return Ok(report);
}



        // Yeni endpoint: employeeId'ye göre employeeProjects getirme
        [HttpGet("by-employee/{employeeId}")]
            public async Task<ActionResult<IEnumerable<EmployeeProject>>> GetEmployeeProjectsByEmployeeId(int employeeId)
            {
                if (_context.EmployeeProjects == null)
                {
                    return NotFound();
                }

                var employeeProjects = await _context.EmployeeProjects
                    .Where(ep => ep.EmployeeId == employeeId)
                    .ToListAsync();

                if (employeeProjects == null || employeeProjects.Count == 0)
                {
                    return NotFound();
                }

                return employeeProjects;
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
                StartDate = employeeProjectDto.StartDate,
                EndDate = employeeProjectDto.EndDate
            });

            var epEffort = await _context.EmployeeProjects.FirstOrDefaultAsync();
            if (epEffort != null)
            {
                _ = epEffort.Effort.ToList(); //NEW
            }
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