using Application.Interfaces;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using Quartz;

namespace Infrastructure.BackgroundJobs;

// Ensures that only one instance of this job can be executed at a time
[DisallowConcurrentExecution]
public class CheckFishingDayHasCompleted(
    DaettwilerPondDbContext dbContext,
    ICatchRepository catchRepository,
    IEmailService emailService,
    ILogger<CheckFishingDayHasCompleted> logger) : IJob
{
    // Implementation of the Execute method from the IJob interface
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            // Retrieve open catches that need to be completed
            var openCatches = await dbContext.Catches
                .AsNoTracking()
                .Where(c => c.StartFishing.HasValue && !c.EndFishing.HasValue && c.StartFishing.Value < DateTime.Now)
                .ToListAsync();

            // If there are no open catches, exit the method
            if (openCatches.Count == 0) return;

            // Iterate through open catches and complete them
            foreach (var fishCatch in openCatches)
            {
                if (fishCatch.StartFishing == null) continue;

                // Calculate the end date for the catch
                var catchDate = fishCatch.StartFishing.Value;
                var endDate = new DateTime(catchDate.Year, catchDate.Month, catchDate.Day).AddDays(1)
                    .AddMinutes(-1);

                // Stop catch day and save to Database
                await catchRepository.StopCatchDayAsync(fishCatch.Id, endDate);

                // Create an email message for the user
                var message = await CreateMailMessage(fishCatch.FishingLicenseId, catchDate);

                // Send the email and log a warning if it fails
                var mailSend = await emailService.SendEmailAsync(message);
                if (!mailSend) logger.LogWarning("Could not send email");
            }
        }
        catch (Exception e)
        {
            // Log any exceptions that occur during execution
            logger.LogError(e, e.Message);
        }
    }

    // Helper method to create an email message for the user
    private async Task<EmailMessage> CreateMailMessage(Guid licenceId, DateTime catchDate)
    {
        // Retrieve user information based on the fishing license ID
        var userWithMail = await dbContext.FishingLicenses
            .AsNoTracking()
            .Include(f => f.User)
            .Where(l => l.Id == licenceId)
            .Select(l => new
            {
                l.User.FirstName,
                l.User.LastName,
                l.User.Email
            }).FirstOrDefaultAsync() ?? throw new ArgumentException($"No User for Licence: {licenceId}");

        // Create the content of the email
        var content = $"<h2>Hallo {userWithMail.FirstName} {userWithMail.LastName},</h2>" +
                      $"<p>Diese Nachricht wurde automatisch generiert, um dich darüber zu informieren, dass dein Fangtag vom {catchDate:dd.MM.yyyy} automatisch abgeschlossen wurde.</p>" +
                      $"<p>Sollten Anpassungen nötig sein, kannst du diese <a href=\"https://weiher.tomasi-developing.ch/home\">direkt im Portal</a> vornehmen. Vielen Dank für deine Kooperation.</p>" +
                      $"<br> " +
                      $"<small>Bitte beachte, dass auf direkte Antworten auf diese E-Mail nicht überwacht wird. Bei weiteren Fragen oder Anliegen stehen wir jedoch gerne zur Verfügung. Nutze dazu bitte die üblichen Kontaktmöglichkeiten.</small>";

        // Return the email message
        return new EmailMessage(new[] { userWithMail.Email }, "Fangtag automatisch abgeschlossen", content);
    }
}