namespace Orders.Api.Outbox;

public class OutboxBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<OutboxBackgroundService> logger) : BackgroundService
{
    private const int OutboxProcessorFrequency = 7;

    protected override async Task ExecuteAsync(CancellationToken ctx)
    {
        try
        {
            logger.LogInformation("Starting OutboxBackgroundService...");

            while (!ctx.IsCancellationRequested)
            {
                using var scope = serviceScopeFactory.CreateScope();
                var outboxProcessor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();

                await outboxProcessor.Execute(ctx);

                await Task.Delay(TimeSpan.FromSeconds(OutboxProcessorFrequency), ctx);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("OutboxBackgroundService cancelled.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured in OutboxBackgroundService");
        }
        finally
        {
            logger.LogInformation("OutboxBackgroundService finished...");
        }
    }
}