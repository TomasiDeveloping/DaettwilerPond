using Application.Interfaces;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class CheckFishingDayHasCompleted(
    DaettwilerPondDbContext dbContext,
    IEmailService emailService,
    ILogger<CheckFishingDayHasCompleted> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var openCatches = await dbContext.Catches
                .Where(c => c.StartFishing.HasValue && !c.EndFishing.HasValue && c.StartFishing.Value < DateTime.Now)
                .ToListAsync();
            if (openCatches.Count == 0) return;

            foreach (var fishCatch in openCatches)
            {
                if (fishCatch.StartFishing != null)
                {
                    var catchDate = fishCatch.StartFishing.Value;
                    var endDate = new DateTime(catchDate.Year, catchDate.Month, catchDate.Day).AddDays(1)
                        .AddMinutes(-1);
                    fishCatch.EndFishing = endDate;
                    var message = await CreateMailMessage(fishCatch.FishingLicenseId, catchDate);
                    var mailSend = await emailService.SendEmailAsync(message);
                    if (!mailSend) logger.LogWarning("Could not send email");
                }

                await dbContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }

    private async Task<EmailMessage> CreateMailMessage(Guid licenceId, DateTime catchDate)
    {
        var userWithMail = await dbContext.FishingLicenses
            .Include(f => f.User)
            .Where(l => l.Id == licenceId)
            .Select(l => new
            {
                l.User.FirstName,
                l.User.LastName,
                l.User.Email
            }).FirstOrDefaultAsync() ?? throw new ArgumentException($"No User for Licence: {licenceId}");

        var content = $"<h1>Hallo {userWithMail.FirstName} {userWithMail.LastName}</h1>" +
                      $"<p>Dein Fangtag vom {catchDate:dd.MM.yyyy} wurde automatisch abgeschlossen.</p>" +
                      $"<p>Bitte korrigiere fals nötig den Tag im Portal. Danke</p>";
        return new EmailMessage(new[] { userWithMail.Email }, "Fangtag automatisch abgeschlossen", content);
    }
}