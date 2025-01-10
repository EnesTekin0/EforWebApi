using Microsoft.AspNetCore.Mvc;
using EforWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using EforWebApi.DTO;
using System;
using System.Threading.Tasks;


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

        [HttpGet("ByWeek/{employeeProjectId}/{effortDate}")]
        public async Task<ActionResult<IEnumerable<EffortDto>>> GetEffortsByWeek(int employeeProjectId, DateTime effortDate)
        {
            var startDate = effortDate.AddDays(-(int)effortDate.DayOfWeek + (int)DayOfWeek.Monday); // Haftanın başlangıcı
            var endDate = startDate.AddDays(4); // Haftanın sonu

            // Günlük eforları gruplama ve toplama
            var efforts = await _context.Efforts
                .Where(e => e.EmployeeProjectId == employeeProjectId && e.EffortDate.Date >= startDate && e.EffortDate.Date <= endDate)
                .GroupBy(e => e.EffortDate.Date) // Tarih bazında gruplandırma
                .Select(group => new EffortDto
                {
                    EmployeeProjectId = employeeProjectId,
                    EffortDate = group.Key, // Grup anahtarı (tarih)
                    EffortAmount = group.Sum(e => e.EffortAmount) // Aynı güne ait eforların toplamı
                })
                .ToListAsync();

            // Haftanın günlerini doldurmak için eksik günleri ekleme
            var result = new List<EffortDto>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dailyEffort = efforts.FirstOrDefault(e => e.EffortDate.Date == date.Date);

                result.Add(new EffortDto
                {
                    EmployeeProjectId = employeeProjectId,
                    EffortDate = date,
                    EffortAmount = dailyEffort?.EffortAmount ?? 0 // Eğer o gün için efor yoksa 0 göster
                });
            }

            return Ok(result);
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

        [HttpPost]
        public async Task<ActionResult<Effort>> PostEffort(EffortDto effortDto)
        {
            if (effortDto == null)
            {
                return BadRequest("Effort data is null.");
            }

            // Gelen tarihi UTC olarak kabul ediyoruz
            var utcEffortDate = DateTime.SpecifyKind(effortDto.EffortDate, DateTimeKind.Utc);

            // UTC'yi Türkiye saati (UTC+3) olarak dönüştürme
            var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var effortDateInIstanbulTime = TimeZoneInfo.ConvertTimeFromUtc(utcEffortDate, turkeyTimeZone);

            var result = _context.Efforts.Add(new Effort
            {
                EffortAmount = effortDto.EffortAmount,
                EffortDate = effortDateInIstanbulTime, // Türkiye saatiyle kaydediyoruz
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