using FastEndpoints;
using Home.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Home.Api.Property
{
    public class SearchProperties : Endpoint<PropertySearchRequest, List<PropertySearchResponse>>
    {
        ApplicationDbContext _context;
        
        public SearchProperties(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/Property/Search");
            AuthSchemes("ApiKeyScheme");
        }

        public override async Task HandleAsync(PropertySearchRequest req, CancellationToken ct)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var isArabic = culture == "ar";

            var query = _context.Properties
                .Where(p => !p.DeletedDate.HasValue && p.IsActive && p.IsAvailable);

            // Apply filters
            if (!string.IsNullOrEmpty(req.SearchText))
            {
                query = query.Where(p => 
                    p.Title.Contains(req.SearchText) || 
                    (p.TitleAr != null && p.TitleAr.Contains(req.SearchText)) ||
                    (p.Description != null && p.Description.Contains(req.SearchText)) ||
                    (p.DescriptionAr != null && p.DescriptionAr.Contains(req.SearchText)));
            }

            if (req.MinPrice.HasValue)
                query = query.Where(p => p.Price >= req.MinPrice.Value);

            if (req.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= req.MaxPrice.Value);

            if (req.CountryId.HasValue)
                query = query.Where(p => p.CountryId == req.CountryId.Value);

            if (req.CityId.HasValue)
                query = query.Where(p => p.CityId == req.CityId.Value);

            if (req.DistrictId.HasValue)
                query = query.Where(p => p.DistrictId == req.DistrictId.Value);

            if (req.RealStateTypeId.HasValue)
                query = query.Where(p => p.RealStateTypeId == req.RealStateTypeId.Value);

            if (req.RealStatePurposeId.HasValue)
                query = query.Where(p => p.RealStatePurposeId == req.RealStatePurposeId.Value);

            if (req.MinBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= req.MinBedrooms.Value);

            if (req.MaxBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms <= req.MaxBedrooms.Value);

            if (req.MinArea.HasValue)
                query = query.Where(p => p.Area >= req.MinArea.Value);

            if (req.MaxArea.HasValue)
                query = query.Where(p => p.Area <= req.MaxArea.Value);

            if (req.HasParking.HasValue)
                query = query.Where(p => p.HasParking == req.HasParking.Value);

            if (req.HasElevator.HasValue)
                query = query.Where(p => p.HasElevator == req.HasElevator.Value);

            if (req.IsFeatured.HasValue)
                query = query.Where(p => p.IsFeatured == req.IsFeatured.Value);

            // Apply ordering
            query = req.SortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "area_asc" => query.OrderBy(p => p.Area),
                "area_desc" => query.OrderByDescending(p => p.Area),
                "date_asc" => query.OrderBy(p => p.InsertedDate),
                "date_desc" => query.OrderByDescending(p => p.InsertedDate),
                _ => query.OrderByDescending(p => p.InsertedDate) // Default: newest first
            };

            // Apply pagination
            var pageSize = req.PageSize ?? 20;
            var pageNumber = req.PageNumber ?? 1;
            var skip = (pageNumber - 1) * pageSize;

            var properties = await query
                .Skip(skip)
                .Take(pageSize)
                .Include(p => p.Images)
                .Select(p => new PropertySearchResponse
                {
                    Id = p.Id,
                    Title = isArabic && !string.IsNullOrEmpty(p.TitleAr) ? p.TitleAr : p.Title,
                    Description = isArabic && !string.IsNullOrEmpty(p.DescriptionAr) ? p.DescriptionAr : p.Description,
                    Price = p.Price,
                    IsNegotiable = p.IsNegotiable,
                    Area = p.Area,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    FloorNumber = p.FloorNumber,
                    HasParking = p.HasParking,
                    HasElevator = p.HasElevator,
                    IsFeatured = p.IsFeatured,
                    CountryId = p.CountryId,
                    CityId = p.CityId,
                    DistrictId = p.DistrictId,
                    RealStateTypeId = p.RealStateTypeId,
                    RealStatePurposeId = p.RealStatePurposeId,
                    ContactPhone = p.ContactPhone,
                    ContactEmail = p.ContactEmail,
                    InsertedDate = p.InsertedDate,
                    MainImageUrl = p.Images.FirstOrDefault(i => i.IsMainImage) != null 
                        ? p.Images.FirstOrDefault(i => i.IsMainImage)!.ImageUrl 
                        : p.Images.FirstOrDefault() != null 
                            ? p.Images.FirstOrDefault()!.ImageUrl 
                            : null,
                    ImageCount = p.Images.Count
                })
                .ToListAsync(ct);

            await SendAsync(properties, cancellation: ct);
        }
    }

    public class PropertySearchRequest
    {
        public string? SearchText { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? RealStateTypeId { get; set; }
        public int? RealStatePurposeId { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public decimal? MinArea { get; set; }
        public decimal? MaxArea { get; set; }
        public bool? HasParking { get; set; }
        public bool? HasElevator { get; set; }
        public bool? IsFeatured { get; set; }
        public string? SortBy { get; set; } // price_asc, price_desc, area_asc, area_desc, date_asc, date_desc
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 20;
    }

    public class PropertySearchResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsNegotiable { get; set; }
        public decimal? Area { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? FloorNumber { get; set; }
        public bool HasParking { get; set; }
        public bool HasElevator { get; set; }
        public bool IsFeatured { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public int RealStateTypeId { get; set; }
        public int RealStatePurposeId { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public DateTime InsertedDate { get; set; }
        public string? MainImageUrl { get; set; }
        public int ImageCount { get; set; }
    }
}