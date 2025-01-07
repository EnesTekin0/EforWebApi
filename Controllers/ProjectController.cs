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
        public async Task<ActionResult<Project>> PostProject([FromBody] ProjectDto projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Project data is null.");
            }

            var project = new Project
            {
                ProjectName = projectDto.ProjectName,
                StartDate = projectDto.StartDate,
                EndDate = projectDto.EndDate,
                ActiveProjects = projectDto.ActiveProjects,
                GitHubLink = projectDto.GitHubLink,
                JiraLink = projectDto.JiraLink,
                ProdLink = projectDto.ProdLink,
                PreProdLink = projectDto.PreProdLink,
                TestLink = projectDto.TestLink
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project);
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, [FromBody] ProjectDto projectDto)
        {
            if (!_context.Projects.Any(p => p.ProjectId == id))
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.ProjectName = projectDto.ProjectName;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;
            project.ActiveProjects = projectDto.ActiveProjects;
            project.GitHubLink = projectDto.GitHubLink;
            project.JiraLink = projectDto.JiraLink;
            project.ProdLink = projectDto.ProdLink;
            project.PreProdLink = projectDto.PreProdLink;
            project.TestLink = projectDto.TestLink;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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