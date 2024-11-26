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
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // POST: api/Project
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Employee data is null.");
            }
            var result = _context.Projects.Add(new Project
            {
                ProjectName = projectDto.ProjectName,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                ActiveProjects = projectDto.ActiveProjects
            });
            await _context.SaveChangesAsync();
            return Ok(result.Entity);
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectDto projectDto)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            project.ProjectName = projectDto.ProjectName;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;
            project.ActiveProjects = projectDto.ActiveProjects;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Projects.Any(p => p.ProjectId == id))
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

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(p => p.ProjectId == id);
        }
    }
}