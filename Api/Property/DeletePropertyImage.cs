using FastEndpoints;
using Home.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Home.Api.PropertyManagement
{
    public class DeletePropertyImageRequest
    {
        public int ImageId { get; set; }
    }

    public class DeletePropertyImageResponse : PublicResult
    {
    }

    public class DeletePropertyImage : Endpoint<DeletePropertyImageRequest, DeletePropertyImageResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DeletePropertyImage(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public override void Configure()
        {
            Delete("/api/property/image/{ImageId}");
            Description(b => b
                .Produces<DeletePropertyImageResponse>(200, "application/json")
                .Produces(400)
                .Produces(404)
            );
        }

        public override async Task HandleAsync(DeletePropertyImageRequest req, CancellationToken ct)
        {
            // Find the image
            var image = await _context.PropertyImages
                .FirstOrDefaultAsync(i => i.Id == req.ImageId, ct);

            if (image == null)
            {
                await SendAsync(new DeletePropertyImageResponse
                {
                    status = false,
                    message = "Image not found"
                }, 404, ct);
                return;
            }

            try
            {
                // Delete the physical file
                var filePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Mark as deleted instead of removing completely
                image.DeletedDate = DateTime.UtcNow;
                image.DeletedBy = "API";
                await _context.SaveChangesAsync(ct);

                // Return success response
                await SendAsync(new DeletePropertyImageResponse
                {
                    status = true,
                    message = "Image deleted successfully"
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new DeletePropertyImageResponse
                {
                    status = false,
                    message = $"Error deleting image: {ex.Message}"
                }, 400, ct);
            }
        }
    }
}