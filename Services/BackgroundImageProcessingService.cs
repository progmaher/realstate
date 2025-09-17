using Home.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Home.Services
{
    public class BackgroundImageProcessingService : BackgroundService
    {
        private readonly ILogger<BackgroundImageProcessingService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _environment;

        public BackgroundImageProcessingService(
            ILogger<BackgroundImageProcessingService> logger,
            IServiceProvider serviceProvider,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _environment = environment;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Image Processing Service is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessImagesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing images");
                }

                // Delay for 1 minute before checking for new images again
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("Background Image Processing Service is stopping");
        }

        private async Task ProcessImagesAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Processing pending images");

            // Use a scope to resolve scoped services
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Get all property images that need processing (you might want to add a flag for this)
            // For demonstration, we'll just get recent unprocessed images
            var recentImages = await dbContext.PropertyImages
                .Where(pi => pi.ProcessedDate == null)
                .Take(10) // Process 10 at a time
                .ToListAsync(stoppingToken);

            foreach (var image in recentImages)
            {
                try
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    var imagePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                    if (!File.Exists(imagePath))
                    {
                        _logger.LogWarning($"Image file not found at path: {imagePath}");
                        continue;
                    }

                    // Create thumbnails directory if it doesn't exist
                    var originalDir = Path.GetDirectoryName(imagePath);
                    var thumbnailsDir = Path.Combine(originalDir, "thumbnails");
                    Directory.CreateDirectory(thumbnailsDir);

                    var fileName = Path.GetFileName(imagePath);
                    var thumbnailPath = Path.Combine(thumbnailsDir, fileName);

                    // Create thumbnail with ImageSharp
                    using (var imageFile = Image.Load(imagePath))
                    {
                        imageFile.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(300, 200),
                            Mode = ResizeMode.Max
                        }));
                        
                        await imageFile.SaveAsync(thumbnailPath, stoppingToken);
                    }

                    // Update the database to mark this image as processed
                    image.ProcessedDate = DateTime.UtcNow;
                    image.ThumbnailUrl = image.ImageUrl.Replace(fileName, $"thumbnails/{fileName}");
                    
                    _logger.LogInformation($"Successfully processed image {image.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing image {image.Id}");
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}