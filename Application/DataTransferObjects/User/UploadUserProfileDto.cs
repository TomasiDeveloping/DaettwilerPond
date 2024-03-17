using Microsoft.AspNetCore.Http;

namespace Application.DataTransferObjects.User;

public class UploadUserProfileDto
{
    public Guid UserId { get; set; }

    public IFormFile File { get; set; }
}