using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class FileController : ControllerBase
{
    private const string UploadFolder = "uploads";
    private readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    private readonly string[] AllowedVideoExtensions = { ".mp4", ".avi", ".mov", ".wmv" };
    private const int MaxFileSizeMB = 100;

    public FileController()
    {
        if (!Directory.Exists(UploadFolder))
        {
            Directory.CreateDirectory(UploadFolder);
        }
    }

    /// <summary>
    /// Upload multiple files to the server
    /// </summary>
    /// <param name="files">The files to upload (supports PNG, JPEG, MP4, AVI, GIF)</param>
    /// <returns>Response indicating success or failure of the upload operation</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Upload multiple files", Description = "Upload image or video files to the server")]
    [SwaggerResponse(200, "Files uploaded successfully", typeof(Response))]
    [SwaggerResponse(400, "Invalid file type or upload error", typeof(Response))]
    [RequestSizeLimit(100 * 1024 * 1024)] // 100MB limit
    public async Task<ActionResult<Response>> Upload(List<IFormFile> files)
    {
        try
        {
            if (files == null || !files.Any())
            {
                return BadRequest("No files were uploaded.");
            }

            var uploadedFiles = new List<string>();
            foreach (var file in files)
            {
                // Validate file size
                if (file.Length > MaxFileSizeMB * 1024 * 1024)
                {
                    return BadRequest($"File {file.FileName} exceeds the maximum size of {MaxFileSizeMB}MB");
                }

                // Validate file extension
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedImageExtensions.Contains(extension) && !AllowedVideoExtensions.Contains(extension))
                {
                    return BadRequest($"File type {extension} is not allowed");
                }

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(UploadFolder, uniqueFileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                uploadedFiles.Add(uniqueFileName);
            }

            return Ok(new Response(true, $"Files uploaded successfully. File names: {string.Join(", ", uploadedFiles)}",uploadedFiles));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult GetSupportedTypes()
    {
        return Ok(new
        {
            Images = AllowedImageExtensions,
            Videos = AllowedVideoExtensions,
            MaxSizeMB = MaxFileSizeMB
        });
    }
}