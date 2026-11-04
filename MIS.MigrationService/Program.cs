using Microsoft.EntityFrameworkCore;
using MIS.API;
using MIS.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AppDbContext>("postgresdb", configureDbContextOptions: options =>
{
    options.UseNpgsql(npgsqlOptions =>
        npgsqlOptions.MigrationsAssembly("MIS.API"));
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();