using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Snackis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("SnackisContextConnection") ?? throw new InvalidOperationException("Connection string 'SnackisContextConnection' not found.");

            builder.Services.AddDbContext<Data.SnackisContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddTransient<DAL.CategoryManager>();

            //Får inte detta att funka
            builder.Services.AddTransient<Services.IHelpers, Services.Helpers>();

            //Så kör så här så länge istället
            builder.Services.AddTransient<Services.Helpers>();

            builder.Services.AddDefaultIdentity<Areas.Identity.Data.SnackisUser>(options =>
            options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<Data.SnackisContext>();

            builder.Services.AddAuthorization(options =>
            options.AddPolicy("AdminKrav", policy => policy.RequireRole("Admin")));

            // Add services to the container.
            builder.Services.AddRazorPages(options => options.Conventions.AuthorizeFolder("/Admin", "AdminKrav"));

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var app = builder.Build();

            app.UseCookiePolicy();

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

            app.Run();
        }
    }
}
