using FastEndpoints;
using Home.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Home.Api.PropertyManagement
{
    public class GetPropertyImagesRequest
    {
        public int PropertyId { get; set; }
    }

    public class PropertyImageDto
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetPropertyImagesResponse : PublicResult
    {
        public List<PropertyImageDto> Images { get; set; } = new List<PropertyImageDto>();
    }

    public class GetPropertyImages : Endpoint<GetPropertyImagesRequest, GetPropertyImagesResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetPropertyImages(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("/api/property/{PropertyId}/images");
            Description(b => b
                .Produces<GetPropertyImagesResponse>(200, "application/json")
                .Produces(400)
            );
        }

        public override async Task HandleAsync(GetPropertyImagesRequest req, CancellationToken ct)
        {
            // Check if property exists
            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == req.PropertyId, ct);

            if (property == null)
            {
                await SendAsync(new GetPropertyImagesResponse
                {
                    status = false,
                    message = "Property not found"
                }, cancellation: ct);
                return;
            }

            // Get all images for the property
            var images = await _context.PropertyImages
                .Where(i => i.PropertyId == req.PropertyId && i.DeletedDate == null)
                .OrderByDescending(i => i.IsMainImage)
                .ThenByDescending(i => i.InsertedDate)
                .Select(i => new PropertyImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    IsMain = i.IsMainImage,
                    CreatedAt = i.InsertedDate
                })
                .ToListAsync(ct);

            // Return response
            await SendAsync(new GetPropertyImagesResponse
            {
                status = true,
                message = $"Found {images.Count} images",
                Images = images
            }, cancellation: ct);
        }
    }
}