using EforWebApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EforWebApi.DTO
{
    public class EffortDto
    {
        public int EmployeeProjectId { get; set; }
        public DateTime EffortDate { get; set; }
        public decimal MonthlyEffort { get; set; }
    }
}