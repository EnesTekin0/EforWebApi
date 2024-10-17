namespace EforWebApi.DTO
{
    public class EmployeeDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Groups { get; set; }
        public DateTime HireDate { get; set; } = DateTime.Now.ToUniversalTime();
        public bool InactiveEmployees { get; set; }
    }
}