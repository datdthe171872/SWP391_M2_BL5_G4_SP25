using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Models;
using SWP391_M2_BL5_G4_SP25.Service;

namespace SWP391_M2_BL5_G4_SP25
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MyDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("value")));
          

            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<MyDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(30); 
                options.SlidingExpiration = true;
            });
            builder.Services.AddScoped<EmailSender>();
            builder.Services.AddScoped<AdminDashService>();
            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string[] roleNames = { "Admin", "Client", "JobSeeker" };

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        var newRole = new Role
                        {
                            Name = roleName,
                        };
                        await roleManager.CreateAsync(newRole);
                    }
                }

                // ✅ Tạo user admin mặc định (tùy chọn)
                var adminEmail = "admin@example.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdmin = new User
                    {
                        UserName = adminEmail,
                        FullName = "Admin",
                        Email = adminEmail,

                    };

                    var result = await userManager.CreateAsync(newAdmin, "Admin@123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAdmin, "Admin");
                    }
                }
            }
            app.Run();
        }
    }
}
