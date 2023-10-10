using la_mia_pizzeria_static.CustomLoggers;
using la_mia_pizzeria_static.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json.Serialization;

namespace la_mia_pizzeria_static
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencySymbol = "€";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddDbContext<DbPizzeriaContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DbPizzeriaContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Codice di cofigurazione per il serializzatore JSON, in modo che ignori completamente le dipendenze cicliche di
            // eventuali relazione N:N o 1:N presenti nel JSON risultante.
            builder.Services.AddControllers().AddJsonOptions(x =>
                            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddScoped<ICustomLogger, CustomFileLogger>();
            builder.Services.AddScoped<DbPizzeriaContext, DbPizzeriaContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=UserIndex}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}