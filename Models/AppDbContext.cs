using Microsoft.EntityFrameworkCore;

namespace EforWebApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<Effort> Efforts { get; set; }
        public DbSet<Project> Projects { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=EforDB;Username=postgres;Password=1234");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee - EmployeeProject (1 to many)
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.EmployeeProjects)
                .WithOne(ep => ep.Employee)
                .HasForeignKey(ep => ep.EmployeeId);

            // Project - EmployeeProject (1 to many)
            modelBuilder.Entity<Project>()
                .HasMany(p => p.EmployeeProjects)
                .WithOne(ep => ep.Project)
                .HasForeignKey(ep => ep.ProjectId);

            // Employee - Effort (1 to many)
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Efforts)
                .WithOne(er => er.Employee)
                .HasForeignKey(er => er.EmployeeId);

            // Project - Effort (1 to many)
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Efforts)
                .WithOne(er => er.Project)
                .HasForeignKey(er => er.ProjectId);
        }
    }
}
