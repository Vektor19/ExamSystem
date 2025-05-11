using ExamSystem.Application.Interfaces.Services;
using ExamSystem.Core.Entities;

public class ExamExpirationChecker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExamExpirationChecker> _logger;

    public ExamExpirationChecker(IServiceProvider serviceProvider, ILogger<ExamExpirationChecker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var examService = scope.ServiceProvider.GetRequiredService<IExamService>();

            var now = DateTime.UtcNow;
            var expiredResult = await examService.GetExpiredNotFinishedExamUsersAsync(now);

            if (expiredResult.Success)
            {
                foreach (var examUser in expiredResult.Data!)
                {
                    await examService.FinishExamAsync(examUser.ExamId, examUser.UserId);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
