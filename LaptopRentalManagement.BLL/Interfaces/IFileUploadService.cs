using Microsoft.AspNetCore.Http;

namespace LaptopRentalManagement.BLL.Interfaces;

public interface IFileUploadService
{
    /// <summary>
    /// Uploads an image file and returns the relative URL path
    /// </summary>
    /// <param name="imageFile">The image file to upload</param>
    /// <param name="folder">The subfolder within wwwroot/images to save the file</param>
    /// <returns>The relative URL path to the uploaded image</returns>
    Task<string?> UploadImageAsync(IFormFile imageFile, string folder = "laptops");
    
    /// <summary>
    /// Deletes an uploaded image file
    /// </summary>
    /// <param name="imageUrl">The relative URL path of the image to delete</param>
    /// <returns>True if deletion was successful</returns>
    Task<bool> DeleteImageAsync(string imageUrl);
}