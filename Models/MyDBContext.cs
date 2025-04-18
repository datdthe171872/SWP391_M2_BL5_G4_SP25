using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Models.Enum;

namespace SWP391_M2_BL5_G4_SP25.Models
{
    public class MyDBContext : IdentityDbContext<User,Role,int>
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }
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
            base.OnModelCreating(modelBuilder); // RẤT QUAN TRỌNG!
            // Configure enums as strings

            modelBuilder.Entity<Job>()
                .Property(j => j.JobType)
                .HasConversion<string>();

            modelBuilder.Entity<Job>()
                .Property(j => j.Status)
                .HasConversion<string>();

            modelBuilder.Entity<JobApplication>()
                .Property(ja => ja.Status)
                .HasConversion<string>();

            // Seed data for JobCategories
            modelBuilder.Entity<JobCategory>().HasData(
                new JobCategory { JobCategoryID = 1, CategoryName = "Information Technology", Description = "Programming, software development, network administration" },
                new JobCategory { JobCategoryID = 2, CategoryName = "Marketing", Description = "Marketing, advertising, communications" },
                new JobCategory { JobCategoryID = 3, CategoryName = "Business", Description = "Sales, customer management, business development" },
                new JobCategory { JobCategoryID = 4, CategoryName = "Human Resources", Description = "Recruitment, training, personnel management" },
                new JobCategory { JobCategoryID = 5, CategoryName = "Accounting - Finance", Description = "Accounting, auditing, financial analysis" }
            );


        }
    }
}
