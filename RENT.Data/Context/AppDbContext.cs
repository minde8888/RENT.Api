using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using WTP.Domain.Entities;
using WTP.Domain.Entities.Auth;
using WTP.Domain.Entities.Roles;

namespace WTP.Data.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public DbSet<Manager> Manager { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProgressPlan> ProgressPlan { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<EmployeeProgressPlan> EmployeeProgressPlan { get; set; }
        public DbSet<Rent> Rent { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");

            //Global Query Filters
            builder.Entity<Manager>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Employee>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Project>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<ProgressPlan>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<ApplicationUser>().HasQueryFilter(p => p.IsDeleted == false);

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("UserLogins");
            });

            builder.Entity<EmployeeProgressPlan>()
                .HasKey(i => new { i.EmployeesId, i.ProgressPlanId });

            builder.Entity<Employee>()
           .HasMany(x => x.ProgressPlans)
           .WithMany(x => x.Employees)
           .UsingEntity<EmployeeProgressPlan>(
               x => x.HasOne(x => x.ProgressPlans)
               .WithMany().HasForeignKey(x => x.ProgressPlanId),
               x => x.HasOne(x => x.Employees)
              .WithMany().HasForeignKey(x => x.EmployeesId));
        }
    }
}