namespace Presentation.WebApp.Services.Interfaces;

public interface IImageUploadService
{
    void DeleteImage(string imagePath);
    Task<string?> UploadImageAsync(IFormFile file, string folderName);
}