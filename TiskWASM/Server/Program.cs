using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using TiskWASM.Server.Data;

namespace TiskWASM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews().AddJsonOptions(opt => {
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opt.JsonSerializerOptions.WriteIndented = true;
            });

            builder.Services.AddRazorPages();

            var config = builder.Configuration.GetConnectionString("PrimaryDb");
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlite(config);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
