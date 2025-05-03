namespace Presentation.WebApp.Services.Interfaces;

public interface IImageUploadService
{
    void DeleteImage(string imagePath);
    Task<string?> UpdateImageAsync(IFormFile file, string folderName, string oldImagePath);
    Task<string?> UploadImageAsync(IFormFile file, string folderName);
}