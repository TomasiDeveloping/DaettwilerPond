using Application.DataTransferObjects.Catch;
using Application.Interfaces;
using Asp.Versioning;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

// Define the route, API version, and authorize the controller
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class OverseersController(
    IFishingLicenseRepository fishingLicenseRepository,
    IUserRepository userRepository,
    IReportService reportService,
    ILogger<OverseersController> logger) : ControllerBase
{
    [HttpGet("{year:int}")]
    public async Task<ActionResult<DetailYearlyCatch>> GetDetailYearlyCatch(int year)
    {
        try
        {
            var detailYearlyCatch = await fishingLicenseRepository.GetDetailYearlyCatchAsync(year);
            return detailYearlyCatch is null ? NotFound($"No Details for year {year}") : Ok(detailYearlyCatch);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetDetailYearlyCatch)}");
        }
    }

    [HttpGet("{year:int}/{userId:guid}")]
    public async Task<ActionResult<DetailYearlyCatch>> GetMemberDetail(int year, Guid userId)
    {
        try
        {
            // Retrieve detailed yearly catch information for the specified user
            var detailYearlyCatch = await fishingLicenseRepository.GetOverseerMemberDetailAsync(userId, year);
            return detailYearlyCatch is null ? NotFound($"No Details for userId {userId}") : Ok(detailYearlyCatch);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetDetailYearlyCatch)}");
        }
    }

    [HttpGet("{year:int}")]
    public async Task<IActionResult> GetYearlyExcelReport(int year)
    {
        try
        {
            // Create yearly Excel report for the specified year
            var workBook = await reportService.CreateYearlyExcelReportAsync(year);

            // Save the workbook to a memory stream
            await using var stream = new MemoryStream();
            workBook.SaveAs(stream, new SaveOptions() {GenerateCalculationChain = false});

            // Get the content of the memory stream
            var content = stream.ToArray();

            // Define the filename for the Excel file
            var filename = $"Fangstatistik_{year}.xlsx";

            // Set response headers for file download
            Response.Headers.Append("x-file-name", filename);
            Response.Headers.Append("Access-Control-Expose-Headers", "x-file-name");

            // Return the Excel file as a downloadable file
            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                filename);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetYearlyExcelReport)}");
        }
    }

    [AllowAnonymous]
    [HttpGet("{year:int}/{userId:guid}")]
    public async Task<IActionResult> GetYearlyUserExcelReport(int year, Guid userId)
    {
        try
        {
            // Retrieve user from the repository
            var user = await userRepository.GetUserByIdAsync(userId);

            // Check if user exists
            if (user is null) return BadRequest("No user found");

            // Create yearly Excel report for the user
            var workBook = await reportService.CreateYearlyUserExcelReportAsync(userId, year);

            // Save the workbook to a memory stream
            await using var stream = new MemoryStream();
            workBook.SaveAs(stream, new SaveOptions() { GenerateCalculationChain = false });

            // Get the content of the memory stream
            var content = stream.ToArray();

            // Define the filename for the Excel file
            var filename = $"Statistik_{user.FirstName}_{user.LastName}_{year}.xlsx";

            // Set response headers for file download
            Response.Headers.Append("x-file-name", filename);
            Response.Headers.Append("Access-Control-Expose-Headers", "x-file-name");

            // Return the Excel file as a downloadable file
            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                filename);
        }
        catch (Exception e)
        {
            // Log and return a generic error response
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Something went wrong in {nameof(GetYearlyExcelReport)}");
        }
    }
}