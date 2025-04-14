using Microsoft.EntityFrameworkCore;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<CompanyReview> CompanyReviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("value"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure enums as strings
            modelBuilder.Entity<Skills>()
                .Property(s => s.ProficiencyLevel)
                .HasConversion<string>();

            modelBuilder.Entity<Job>()
                .Property(j => j.JobType)
                .HasConversion<string>();

            modelBuilder.Entity<Job>()
                .Property(j => j.Status)
                .HasConversion<string>();

            modelBuilder.Entity<JobApplication>()
                .Property(ja => ja.Status)
                .HasConversion<string>();

            // Seed data for Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "JobSeeker", Description = "User looking for job opportunities" },
                new Role { RoleID = 2, RoleName = "Employer", Description = "User representing a company posting jobs" },
                new Role { RoleID = 3, RoleName = "Admin", Description = "System administrator" }
            );

            // Seed data for JobCategories
            modelBuilder.Entity<JobCategory>().HasData(
                new JobCategory { JobCategoryID = 1, CategoryName = "Software Development", Description = "Jobs related to software engineering and development" },
                new JobCategory { JobCategoryID = 2, CategoryName = "Marketing", Description = "Jobs in marketing and advertising" },
                new JobCategory { JobCategoryID = 3, CategoryName = "Finance", Description = "Jobs in finance and accounting" },
                new JobCategory { JobCategoryID = 4, CategoryName = "Human Resources", Description = "Jobs in HR and recruitment" },
                new JobCategory { JobCategoryID = 5, CategoryName = "Design", Description = "Jobs in graphic and UI/UX design" }
            );
        }
    }
}
