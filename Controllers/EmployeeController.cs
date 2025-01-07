using Microsoft.AspNetCore.Mvc;
using EforWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using EforWebApi.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EforWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public EmployeeController(AppDbContext appdbcontext, IConfiguration configuration)
        {
            _context = appdbcontext;
            _configuration = configuration;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            return await _context.Employees.ToListAsync();
        }
       

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == loginDto.Email && e.Password == loginDto.Password);

            if (employee == null)
            {
                return Unauthorized(new { message = "Geçersiz e-posta veya şifre." });
            }

            var role = employee.Role;

            var token = GenerateJwtToken(employee);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Employee employee)
        {

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, employee.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("employeeId", employee.EmployeeId.ToString()),

                    new Claim("role", employee.Role.ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is null.");
            }

            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == employeeDto.Email);
            if (existingEmployee != null)
            {
                return BadRequest("An employee with this email already exists.");
            }
            Role role;
    if (!Enum.TryParse(employeeDto.Role.ToString(), true, out role)) 
    {
        role = Role.User; 
    }


            var result = _context.Employees.Add(new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Password = employeeDto.Password,
                Groups = employeeDto.Groups,
                HireDate = employeeDto.HireDate,
                ActiveEmployees = employeeDto.ActiveEmployees,

                Role = role.ToString() 

            });
            await _context.SaveChangesAsync();
            return Ok(result.Entity);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.Email = employeeDto.Email;
            employee.Password = employeeDto.Password;
            employee.Groups = employeeDto.Groups;
            employee.HireDate = employeeDto.HireDate;
            employee.ActiveEmployees = employeeDto.ActiveEmployees;
            var role = Enum.TryParse<Role>(employeeDto.Role.ToString(), out var parsedRole) ? parsedRole : Role.User;
            employee.Role = role.ToString();


            try
            {
                await _context.SaveChangesAsync();
            }
            catch 
            {
                if (!_context.Employees.Any(e => e.EmployeeId == id))
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

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}