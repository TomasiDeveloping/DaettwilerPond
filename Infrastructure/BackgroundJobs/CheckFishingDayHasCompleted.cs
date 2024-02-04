﻿using Application.Interfaces;
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
                .Where(c => c.StartFishing.HasValue && !c.EndFishing.HasValue && c.StartFishing.Value < DateTime.Now)
                .ToListAsync();

            // If there are no open catches, exit the method
            if (openCatches.Count == 0) return;

            // Iterate through open catches and complete them
            foreach (var fishCatch in openCatches)
            {
                if (fishCatch.StartFishing != null)
                {
                    // Calculate the end date for the catch
                    var catchDate = fishCatch.StartFishing.Value;
                    var endDate = new DateTime(catchDate.Year, catchDate.Month, catchDate.Day).AddDays(1)
                        .AddMinutes(-1);
                    fishCatch.EndFishing = endDate;

                    // Create an email message for the user
                    var message = await CreateMailMessage(fishCatch.FishingLicenseId, catchDate);

                    // Send the email and log a warning if it fails
                    var mailSend = await emailService.SendEmailAsync(message);
                    if (!mailSend) logger.LogWarning("Could not send email");
                }

                // Save changes to the database
                await dbContext.SaveChangesAsync();
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
            .Include(f => f.User)
            .Where(l => l.Id == licenceId)
            .Select(l => new
            {
                l.User.FirstName,
                l.User.LastName,
                l.User.Email
            }).FirstOrDefaultAsync() ?? throw new ArgumentException($"No User for Licence: {licenceId}");

        // Create the content of the email
        var content = $"<h1>Hallo {userWithMail.FirstName} {userWithMail.LastName}</h1>" +
                      $"<p>Dein Fangtag vom {catchDate:dd.MM.yyyy} wurde automatisch abgeschlossen.</p>" +
                      $"<p>Bitte korrigiere fals nötig den Tag im Portal. Danke</p>";

        // Return the email message
        return new EmailMessage(new[] { userWithMail.Email }, "Fangtag automatisch abgeschlossen", content);
    }
}