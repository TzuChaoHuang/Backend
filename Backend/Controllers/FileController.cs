using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class FileController : ControllerBase
{
    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
    private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png" };
    private readonly string[] _allowedVideoExtensions = { ".mp4", ".avi" };

    /// <summary>
    /// Upload a file to the server
    /// </summary>
    /// <param name="file">The file to upload (supports PNG, JPEG, MP4, AVI)</param>
    /// <returns>Response indicating success or failure of the upload operation</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Upload a file", Description = "Upload image or video files to the server")]
    [SwaggerResponse(200, "File uploaded successfully", typeof(Response))]
    [SwaggerResponse(400, "Invalid file type or upload error", typeof(Response))]
    [RequestSizeLimit(70 * 1024 * 1024)] // 70MB limit
    public async Task<ActionResult<Response>> Upload(IFormFile file)
    {
        try
        {
            // Check if file is provided
            if (file == null || file.Length == 0)
                return BadRequest(new Response(false, "No file provided."));

            // Validate file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedImageExtensions.Contains(extension) && !_allowedVideoExtensions.Contains(extension))
                return BadRequest(new Response(false, "Invalid file type. Only PNG, JPEG, MP4, and AVI are allowed."));

            // Ensure upload directory exists
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            // Generate unique file name to avoid overwrites
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadPath, fileName);

            // Save file to disk asynchronously
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new Response(true, $"File uploaded successfully. File name: {fileName}", new { id = fileName }));
        }
        catch (Exception ex)
        {
            return BadRequest(new Response(false, $"Upload failed: {ex.Message}"));
        }
    }
}