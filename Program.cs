using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Client;

namespace ToDoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
            });

            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TodosDataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(
                (options) =>
                {
                    options.AddPolicy("testCors", (corsBuilder) =>
                    {
                        corsBuilder.WithOrigins("http://localhost:5500").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    });
                }
            );

            var app = builder.Build();

            app.Logger.LogDebug("Debuggggggg");
            app.Logger.LogInformation("Inforrrrrrem");
            app.Logger.LogWarning("Warnnnnnnnig");
            app.Logger.LogCritical("Oopsy Critical");
            app.Logger.LogError("Errorsy");



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("testCors");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
