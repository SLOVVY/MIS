using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MIS.API;

namespace MIS.MigrationService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _lifetime;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Начинаю применение миграций...");

        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var historyRepository = dbContext.GetService<IHistoryRepository>();
            var createScript = historyRepository.GetCreateIfNotExistsScript();
            await dbContext.Database.ExecuteSqlRawAsync(createScript, stoppingToken);

            await dbContext.Database.MigrateAsync(stoppingToken);

            _logger.LogInformation("Миграции успешно применены.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при применении миграций.");
            throw;
        }
        finally
        {
            _lifetime.StopApplication();
        }
    }
}