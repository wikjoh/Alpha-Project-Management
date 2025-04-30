using Presentation.WebApp.Services.Interfaces;
using System.IO;

namespace Presentation.WebApp.Services;

public class ImageUploadService(IWebHostEnvironment env) : IImageUploadService
{
    private readonly IWebHostEnvironment _env = env;

    public async Task<string?> UploadImageAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0)
            return null;

        string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            return null;

        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", folderName);
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine("uploads", folderName, uniqueFileName).Replace("\\", "/");
    }

    public void DeleteImage(string imagePath)
    {
        var fullPath = Path.Combine(_env.WebRootPath, imagePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
