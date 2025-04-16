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

            

        }
    }
}
