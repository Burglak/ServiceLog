using Microsoft.EntityFrameworkCore;
using ServiceLog.Application.Interfaces;
using ServiceLog.Infrastructure.Data;

namespace ServiceLog.Infrastructure.Schedulers
{
    public class SchedulerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SchedulerBackgroundService> _logger;

        public SchedulerBackgroundService(IServiceProvider serviceProvider, ILogger<SchedulerBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scheduler started");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextMidnight = DateTime.Today.AddDays(1);
                var delay = nextMidnight - now;

                _logger.LogInformation($"Waiting {delay.TotalMinutes} minutes until midnight.");

                await Task.Delay(delay, stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    var today = DateTime.Today;

                    var notifications = await db.Notifications
                        .Include(n => n.Vehicle)
                        .Where(n => n.NotifyAt.Date == today)
                        .ToListAsync(stoppingToken);

                    foreach (var notification in notifications)
                    {
                        var vehicleUsers = await db.VehicleUsers
                            .Include(vu => vu.User)
                            .Where(vu => vu.VehicleId == notification.VehicleId)
                            .ToListAsync(stoppingToken);

                        foreach (var vu in vehicleUsers)
                        {
                            var user = vu.User;

                            if (!string.IsNullOrWhiteSpace(user.Email))
                            {
                                await emailService.SendEmailAsync(
                                    user.Email,
                                    $"[ServiceLog] Reminder: {notification.Title}",
                                    $"Hi {user.Username},\n\nYou have a reminder scheduled for today.\n\nTitle: {notification.Title}\nDescription: {notification.Description ?? "No additional details provided."}",
                                    $@"
                                        <p>Hi <strong>{user.Username}</strong>,</p>
                                        <p>You have a reminder scheduled for today.</p>
                                        <p><strong>Title:</strong> {notification.Title}</p>
                                        <p><strong>Description:</strong> {notification.Description ?? "No additional details provided."}</p>
                                        <p>Regards,<br/>ServiceLog Team</p>"
                                );


                                _logger.LogInformation($"Email sent to {user.Email} for notification {notification.Title}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing notifications.");
                }
            }
        }
    }
}
