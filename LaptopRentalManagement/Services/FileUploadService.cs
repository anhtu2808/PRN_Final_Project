using LaptopRentalManagement.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LaptopRentalManagement.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

    public FileUploadService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string?> UploadImageAsync(IFormFile imageFile, string folder = "laptops")
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        // Validate file size
        if (imageFile.Length > _maxFileSize)
            throw new InvalidOperationException("File size exceeds the maximum allowed size of 5MB.");

        // Validate file extension
        var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
            throw new InvalidOperationException("Invalid file type. Only image files are allowed.");

        try
        {
            // Create unique filename to prevent conflicts
            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            
            // Create the upload directory if it doesn't exist
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", folder);
            Directory.CreateDirectory(uploadsFolder);
            
            // Full path for saving the file
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            
            // Return the relative URL path
            return $"/images/{folder}/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error uploading file: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return false;

        try
        {
            // Convert URL to physical path
            var relativePath = imageUrl.TrimStart('/');
            var physicalPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
}