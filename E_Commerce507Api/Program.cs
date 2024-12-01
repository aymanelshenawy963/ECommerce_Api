
using E_Commerce507Api.Data;
using E_Commerce507Api.Models;
using E_Commerce507Api.Repository;
using E_Commerce507Api.Repository.IRepository;
using E_Commerce507Api.Utiltiy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace E_Commerce507Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyAllowSpecificOrigins",
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                  });
            });
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(
            option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
             );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option => option.Password.RequireNonAlphanumeric = false)
        .AddEntityFrameworkStores<ApplicationDbContext>();
           
            builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
            builder.Services.AddScoped<IProductRepository,ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("MyAllowSpecificOrigins");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
