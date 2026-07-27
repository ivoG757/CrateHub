namespace Api.Services;

using Api.Repository.Interfaces;
using Api.Services.Interfaces;
public class FileCleanupService : BackgroundService
{
    const int timeDelay = 30;
    private readonly IServiceScopeFactory _scopeFactory;

    public FileCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }


    private async Task DeleteExpiredFiles()
    {
        using var scope = _scopeFactory.CreateScope();

        var fileRepository = scope.ServiceProvider
            .GetRequiredService<IFileRepository>();

        var fileStorage = scope.ServiceProvider
            .GetRequiredService<IFileStorage>();

        var unitOfWork = scope.ServiceProvider
            .GetRequiredService<IUnitOfWork>();


        var expiredFiles = await fileRepository.GetExpiredFiles();

        foreach (var file in expiredFiles)
        {
            try
            {
                fileStorage.Delete(file.Path);

                fileRepository.Delete(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        await unitOfWork.SaveChangesAsync();
    }


    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DeleteExpiredFiles();

            await Task.Delay(TimeSpan.FromMinutes(timeDelay), stoppingToken);
        }
    }
}