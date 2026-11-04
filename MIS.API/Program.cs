using Microsoft.EntityFrameworkCore;
using MIS.API;
using MIS.API.Abstractions;
using MIS.API.Repositories;
using MIS.API.Services;


namespace MIS.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();

        builder.AddNpgsqlDbContext<AppDbContext>("postgresdb", configureDbContextOptions: options =>
        {
            options.UseNpgsql(npgsqlBuilder =>
            {
                npgsqlBuilder.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSwagger",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });

        builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();
        builder.Services.AddScoped<IDoctorsRepository, DoctorsRepository>();
        builder.Services.AddScoped<IDiseasesRepository, DiseasesRepository>();

        builder.Services.AddScoped<IPatientsService, PatientsService>();
        builder.Services.AddScoped<IDoctorsService, DoctorsService>();
        builder.Services.AddScoped<IDiseasesService, DiseasesService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseMiddleware<Middleware.ExceptionHandlingMiddleware>();

        app.UseCors("AllowSwagger");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.MapDefaultEndpoints();

        app.Run();
    }
}
