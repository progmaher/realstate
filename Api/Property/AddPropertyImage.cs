using FastEndpoints;
using Home.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Home.Api.PropertyManagement
{
    public class AddPropertyImageRequest
    {
        public int PropertyId { get; set; }
        public List<IFormFile>? Images { get; set; }
        public int MainImageIndex { get; set; } = -1; // -1 means no main image
    }

    public class AddPropertyImageResponse : PublicResult
    {
        public List<UploadedImageInfo> UploadedImages { get; set; } = new List<UploadedImageInfo>();
    }
    
    public class UploadedImageInfo
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }

    public class AddPropertyImage : Endpoint<AddPropertyImageRequest, AddPropertyImageResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AddPropertyImage(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public override void Configure()
        {
            Post("/api/property/add-images");
            AllowFileUploads();
            Description(b => b
                .Produces<AddPropertyImageResponse>(200, "application/json")
                .Produces(400)
            );
        }

        public override async Task HandleAsync(AddPropertyImageRequest req, CancellationToken ct)
        {
            // Check if property exists
            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == req.PropertyId, ct);

            if (property == null)
            {
                await SendAsync(new AddPropertyImageResponse
                {
                    status = false,
                    message = "Property not found"
                }, cancellation: ct);
                return;
            }

            // Check if files are valid
            if (req.Images == null || !req.Images.Any())
            {
                await SendAsync(new AddPropertyImageResponse
                {
                    status = false,
                    message = "No image files provided"
                }, cancellation: ct);
                return;
            }

            try
            {
                // Create directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "properties", req.PropertyId.ToString());
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // If MainImageIndex is valid, update all existing images to not be main
                if (req.MainImageIndex >= 0 && req.MainImageIndex < req.Images.Count)
                {
                    var existingImages = await _context.PropertyImages
                        .Where(i => i.PropertyId == req.PropertyId)
                        .ToListAsync(ct);

                    foreach (var image in existingImages)
                    {
                        image.IsMainImage = false;
                    }
                    
                    // Save these changes
                    await _context.SaveChangesAsync(ct);
                }

                var response = new AddPropertyImageResponse
                {
                    status = true,
                    message = "Images uploaded successfully",
                    UploadedImages = new List<UploadedImageInfo>()
                };

                // Process each image
                for (int i = 0; i < req.Images.Count; i++)
                {
                    var image = req.Images[i];
                    bool isMainImage = (i == req.MainImageIndex);
                    
                    // Check file type
                    var allowedFileTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
                    if (!allowedFileTypes.Contains(image.ContentType.ToLower()))
                    {
                        continue; // Skip invalid file types
                    }

                    // Check file size (max 5MB)
                    if (image.Length > 5 * 1024 * 1024)
                    {
                        continue; // Skip files that are too large
                    }

                    // Generate unique filename
                    var fileExtension = Path.GetExtension(image.FileName);
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    var relativePath = $"/images/properties/{req.PropertyId}/{fileName}";

                    // Save file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream, ct);
                    }

                    // Create and save image record
                    var propertyImage = new PropertyImage
                    {
                        PropertyId = req.PropertyId,
                        ImageUrl = relativePath,
                        ImageTitle = Path.GetFileNameWithoutExtension(image.FileName),
                        ImageType = image.ContentType,
                        IsMainImage = isMainImage,
                        DisplayOrder = i,
                        InsertedDate = DateTime.UtcNow,
                        InsertedBy = "API"
                    };

                    _context.PropertyImages.Add(propertyImage);
                    await _context.SaveChangesAsync(ct);

                    // Add to response
                    response.UploadedImages.Add(new UploadedImageInfo
                    {
                        ImageId = propertyImage.Id,
                        ImageUrl = propertyImage.ImageUrl,
                        IsMain = propertyImage.IsMainImage
                    });
                }

                // Return success response
                await SendAsync(response, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new AddPropertyImageResponse
                {
                    status = false,
                    message = $"Error uploading images: {ex.Message}"
                }, cancellation: ct);
            }
        }
    }
}