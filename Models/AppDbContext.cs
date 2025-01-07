using EforWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore;

namespace EforWebApi.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<EmployeeProject> EmployeeProjects { get; set; } = null!;
        public DbSet<Effort> Efforts { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Employee - EmployeeProject (1 to many)


            modelBuilder.Entity<Employee>()

                .HasMany(e => e.EmployeeProjects)
                .WithOne(ep => ep.Employee)
                .HasForeignKey(ep => ep.EmployeeId);
           
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.GitHubLink).HasMaxLength(255);
                entity.Property(e => e.JiraLink).HasMaxLength(255);
                entity.Property(e => e.ProdLink).HasMaxLength(255);
                entity.Property(e => e.PreProdLink).HasMaxLength(255);
                entity.Property(e => e.TestLink).HasMaxLength(255);
            });

            // Project - EmployeeProject (1 to many)
            modelBuilder.Entity<Project>()
                .HasMany(p => p.EmployeeProjects)
                .WithOne(ep => ep.Project)
                .HasForeignKey(ep => ep.ProjectId);

            // EmployeeProject - Effort (1 to many)
            modelBuilder.Entity<EmployeeProject>()
                .HasMany(ep => ep.Effort)
                .WithOne(er => er.EmployeeProject)
                .HasForeignKey(er => er.EmployeeProjectId);
        }
    }
}