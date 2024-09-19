using EforWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EforWebApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<EmployeeProject> EmployeeProjects { get; set; } = null!;
        public DbSet<Effort> Efforts { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Database=EforDB;Username=postgres;Password=1234");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Employee - EmployeeProject (1 to many)
            //modelBuilder.Entity<Employee>()
            //    .HasMany(e => e.EmployeeProjects)
            //    .WithOne(ep => ep.Employee)
            //    .HasForeignKey(ep => ep.EmployeeId);

            //// Project - EmployeeProject (1 to many)
            //modelBuilder.Entity<Project>()
            //    .HasMany(e => e.EmployeeProjects)
            //    .WithOne(ep => ep.Project)
            //    .HasForeignKey(ep => ep.ProjectId);

            //// EmployeeProject - Effort (1 to many)
            //modelBuilder.Entity<EmployeeProject>()
            //    .HasMany(ep => ep.Effort)
            //    .WithOne(er => er.EmployeeProject)
            //    .HasForeignKey(er => er.EmployeeProjectId);
        }
    }
}
