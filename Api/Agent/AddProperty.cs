using FastEndpoints;
using Home.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Home.Api.Agent
{
    public class AddProperty : Endpoint<AddPropertyRequest, PublicResult>
    {
        ILogger<AddProperty> _logger;
        ApplicationDbContext _context;

        public AddProperty(ApplicationDbContext context, ILogger<AddProperty> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/Api/Agent/Property/Add");
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        }

        public override async Task HandleAsync(AddPropertyRequest req, CancellationToken ct)
        {
            _logger.LogInformation("Add Property request received at {DateTime}", DateTime.Now);

            try
            {
                // Get current user ID from JWT token
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    await SendAsync(new PublicResult 
                    { 
                        status = false, 
                        message = "User not authenticated" 
                    }, StatusCodes.Status401Unauthorized);
                    return;
                }

                // Get agent information - for now, we'll use the first active agent
                // You might want to link agents to users differently based on your business logic
                var agent = await _context.Agents.FirstOrDefaultAsync(a => a.IsActive);
                if (agent == null)
                {
                    await SendAsync(new PublicResult 
                    { 
                        status = false, 
                        message = "No active agent found. Please ensure agent registration is complete." 
                    }, StatusCodes.Status403Forbidden);
                    return;
                }

                // Create new property
                var property = new Property
                {
                    Title = req.Title,
                    TitleAr = req.TitleAr,
                    Description = req.Description,
                    DescriptionAr = req.DescriptionAr,
                    Price = req.Price,
                    IsNegotiable = req.IsNegotiable,
                    Area = req.Area,
                    Bedrooms = req.Bedrooms,
                    Bathrooms = req.Bathrooms,
                    LivingRooms = req.LivingRooms,
                    Kitchens = req.Kitchens,
                    FloorNumber = req.FloorNumber,
                    TotalFloors = req.TotalFloors,
                    ApartmentNumber = req.ApartmentNumber,
                    HasElevator = req.HasElevator,
                    HasParking = req.HasParking,
                    ParkingSpaces = req.ParkingSpaces,
                    BuildYear = req.BuildYear,
                    Address = req.Address,
                    AddressDescription = req.AddressDescription,
                    LocationDescription = req.LocationDescription,
                    CountryId = req.CountryId,
                    CityId = req.CityId,
                    DistrictId = req.DistrictId,
                    RealStateTypeId = req.RealStateTypeId,
                    RealStatePurposeId = req.RealStatePurposeId,
                    RealStateRentTypeId = req.RealStateRentTypeId,
                    AgentId = agent.Id,
                    ContactPhone = req.ContactPhone,
                    ContactEmail = req.ContactEmail,
                    VideoUrl = req.VideoUrl,
                    ThreeDTour = req.ThreeDTour,
                    IsFeatured = req.IsFeatured,
                    IsActive = true,
                    IsAvailable = true,
                    InsertedBy = userId,
                    InsertedDate = DateTime.UtcNow
                };

                // Add property to context first
                _context.Properties.Add(property);
                await _context.SaveChangesAsync(ct);

                // For images, temporarily store as JSON until PropertyImage migration is applied
                if (req.Images != null && req.Images.Any())
                {
                    // Store images as JSON in property.Images field if it exists, otherwise log
                    try {
                        // Check if property has Images JSON field using reflection
                        var imagesProperty = property.GetType().GetProperty("Images");
                        if (imagesProperty != null && imagesProperty.PropertyType == typeof(string))
                        {
                            property.GetType().GetProperty("Images").SetValue(property, JsonSerializer.Serialize(req.Images));
                            await _context.SaveChangesAsync(ct);
                        }
                        else
                        {
                            _logger.LogWarning("Images field not available. Images will not be saved.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error saving images as JSON");
                    }
                }

                // Convert amenities to JSON string
                if (req.Amenities != null && req.Amenities.Any())
                {
                    property.Amenities = JsonSerializer.Serialize(req.Amenities);
                    await _context.SaveChangesAsync(ct);
                }

                _logger.LogInformation("Property {PropertyId} created successfully by agent {AgentId}", 
                    property.Id, agent.Id);

                await SendAsync(new PublicResult 
                { 
                    status = true, 
                    message = "Property added successfully",
                    token = property.Id.ToString() // Return property ID as token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding property");
                await SendAsync(new PublicResult 
                { 
                    status = false, 
                    message = "Error adding property. Please try again." 
                }, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
