using Banha_UniverCity.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Banha_UniverCity.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;

using Stripe;
using BFCAI.Models;
using DataAccess.Repository.IRepository.Service;
using DataAccess.Repository.ModelsRepository.Service;
using DataAccess;


namespace Banha_UniverCity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.RegisterDbContext(builder.Configuration);
            builder.Services.RegisterCacheService(builder.Configuration);

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI().AddDefaultTokenProviders();


            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


            builder.Services.RegisterInfrastructure(builder.Configuration);

            //builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            //});


            //builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = "850647333864475";
            //    facebookOptions.AppSecret = "b8d8ff900e7979bbbdc9ba793fcef797";
            //});

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
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

            app.UseAuthorization();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
            pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
