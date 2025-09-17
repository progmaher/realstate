using FastEndpoints;
using Home.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Home.Api.Property
{
    public class GetPropertyDetails : Endpoint<PropertyDetailsRequest, PropertyDetailsResponse?>
    {
        ApplicationDbContext _context;
        
        public GetPropertyDetails(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/Property/Details/{id}");
            AuthSchemes("ApiKeyScheme");
        }

        public override async Task HandleAsync(PropertyDetailsRequest req, CancellationToken ct)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var isArabic = culture == "ar";

            var property = await _context.Properties
                .Where(p => p.Id == req.Id && !p.DeletedDate.HasValue)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(ct);

            if (property == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            // Get location names
            var country = await _context.Countries.FindAsync(property.CountryId);
            var city = await _context.Cities.FindAsync(property.CityId);
            var district = await _context.Districts.FindAsync(property.DistrictId);

            // Get classification names
            var realStateType = await _context.RealStateTypes.FindAsync(property.RealStateTypeId);
            var realStatePurpose = await _context.RealStatePurposes.FindAsync(property.RealStatePurposeId);
            var rentType = property.RealStateRentTypeId.HasValue 
                ? await _context.RentTypes.FindAsync(property.RealStateRentTypeId.Value)
                : null;

            // Get agent information
            var agent = await _context.Agents.FindAsync(property.AgentId);

            var response = new PropertyDetailsResponse
            {
                Id = property.Id,
                Title = isArabic && !string.IsNullOrEmpty(property.TitleAr) ? property.TitleAr : property.Title,
                Description = isArabic && !string.IsNullOrEmpty(property.DescriptionAr) ? property.DescriptionAr : property.Description,
                Price = property.Price,
                IsNegotiable = property.IsNegotiable,
                Area = property.Area,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                LivingRooms = property.LivingRooms,
                Kitchens = property.Kitchens,
                FloorNumber = property.FloorNumber,
                TotalFloors = property.TotalFloors,
                ApartmentNumber = property.ApartmentNumber,
                HasElevator = property.HasElevator,
                HasParking = property.HasParking,
                ParkingSpaces = property.ParkingSpaces,
                BuildYear = property.BuildYear,
                Address = property.Address,
                AddressDescription = property.AddressDescription,
                LocationDescription = property.LocationDescription,
                ContactPhone = property.ContactPhone,
                ContactEmail = property.ContactEmail,
                VideoUrl = property.VideoUrl,
                ThreeDTour = property.ThreeDTour,
                Amenities = property.Amenities,
                IsFeatured = property.IsFeatured,
                IsActive = property.IsActive,
                IsAvailable = property.IsAvailable,
                InsertedDate = property.InsertedDate,
                
                // Location information
                Country = new LocationInfo
                {
                    Id = property.CountryId,
                    Name = country != null ? (isArabic ? country.NameAr : country.NameEn) : "Unknown"
                },
                City = new LocationInfo
                {
                    Id = property.CityId,
                    Name = city != null ? (isArabic ? city.NameAr : city.NameEn) : "Unknown"
                },
                District = new LocationInfo
                {
                    Id = property.DistrictId,
                    Name = district != null ? (isArabic ? district.NameAr : district.NameEn) : "Unknown"
                },
                
                // Classification information
                RealStateType = new ClassificationInfo
                {
                    Id = property.RealStateTypeId,
                    Name = realStateType != null ? (isArabic ? realStateType.NameAr : realStateType.NameEn) : "Unknown"
                },
                RealStatePurpose = new ClassificationInfo
                {
                    Id = property.RealStatePurposeId,
                    Name = realStatePurpose != null ? (isArabic ? realStatePurpose.NameAr : realStatePurpose.NameEn) : "Unknown"
                },
                RentType = rentType != null ? new ClassificationInfo
                {
                    Id = rentType.Id,
                    Name = isArabic ? rentType.NameAr : rentType.NameEn
                } : null,
                
                // Agent information
                Agent = agent != null ? new AgentInfo
                {
                    Id = agent.Id,
                    Name = isArabic ? agent.NameAr : agent.NameEn,
                    LicenseNumber = agent.LicenseNumber,
                    CR = agent.CR,
                    Website = agent.Website
                } : null,
                
                // Images
                Images = property.Images.Select(i => new PropertyImageInfo
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    ThumbnailUrl = i.ThumbnailUrl,
                    ImageTitle = i.ImageTitle,
                    ImageDescription = i.ImageDescription,
                    IsMainImage = i.IsMainImage,
                    DisplayOrder = i.DisplayOrder,
                    ImageType = i.ImageType
                }).OrderBy(i => i.DisplayOrder).ToList()
            };

            await SendOkAsync(response, ct);
        }
    }

    public class PropertyDetailsRequest
    {
        public int Id { get; set; }
    }

    public class PropertyDetailsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsNegotiable { get; set; }
        public decimal? Area { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? LivingRooms { get; set; }
        public int? Kitchens { get; set; }
        public int? FloorNumber { get; set; }
        public int? TotalFloors { get; set; }
        public int? ApartmentNumber { get; set; }
        public bool HasElevator { get; set; }
        public bool HasParking { get; set; }
        public int? ParkingSpaces { get; set; }
        public int? BuildYear { get; set; }
        public string? Address { get; set; }
        public string? AddressDescription { get; set; }
        public string? LocationDescription { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? VideoUrl { get; set; }
        public string? ThreeDTour { get; set; }
        public string? Amenities { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime InsertedDate { get; set; }
        
        public LocationInfo Country { get; set; } = new();
        public LocationInfo City { get; set; } = new();
        public LocationInfo District { get; set; } = new();
        public ClassificationInfo RealStateType { get; set; } = new();
        public ClassificationInfo RealStatePurpose { get; set; } = new();
        public ClassificationInfo? RentType { get; set; }
        public AgentInfo? Agent { get; set; }
        public List<PropertyImageInfo> Images { get; set; } = new();
    }

    public class LocationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ClassificationInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AgentInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LicenseNumber { get; set; }
        public string? CR { get; set; }
        public string? Website { get; set; }
    }

    public class PropertyImageInfo
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string? ImageTitle { get; set; }
        public string? ImageDescription { get; set; }
        public bool IsMainImage { get; set; }
        public int DisplayOrder { get; set; }
        public string? ImageType { get; set; }
    }
}