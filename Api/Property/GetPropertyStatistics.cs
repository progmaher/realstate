using FastEndpoints;
using Home.Data;
using Microsoft.EntityFrameworkCore;

namespace Home.Api.Property
{
    public class GetPropertyStatistics : EndpointWithoutRequest<PropertyStatisticsResponse>
    {
        ApplicationDbContext _context;
        
        public GetPropertyStatistics(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/Property/Statistics");
            AuthSchemes("ApiKeyScheme");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var activeProperties = _context.Properties.Where(p => !p.DeletedDate.HasValue && p.IsActive);

            var stats = new PropertyStatisticsResponse
            {
                TotalActiveProperties = await activeProperties.CountAsync(ct),
                TotalAvailableProperties = await activeProperties.Where(p => p.IsAvailable).CountAsync(ct),
                TotalFeaturedProperties = await activeProperties.Where(p => p.IsFeatured).CountAsync(ct),
                
                // Price statistics
                AveragePrice = await activeProperties.AverageAsync(p => p.Price, ct),
                MinPrice = await activeProperties.MinAsync(p => p.Price, ct),
                MaxPrice = await activeProperties.MaxAsync(p => p.Price, ct),
                
                // Area statistics (only for properties with area data)
                AverageArea = await activeProperties.Where(p => p.Area.HasValue).AverageAsync(p => p.Area!.Value, ct),
                
                // Properties by purpose
                PropertiesForSale = await activeProperties.CountAsync(p => 
                    _context.RealStatePurposes.Any(rsp => rsp.Id == p.RealStatePurposeId && 
                        (rsp.NameEn.ToLower().Contains("sale") || rsp.NameAr.Contains("بيع"))), ct),
                
                PropertiesForRent = await activeProperties.CountAsync(p => 
                    _context.RealStatePurposes.Any(rsp => rsp.Id == p.RealStatePurposeId && 
                        (rsp.NameEn.ToLower().Contains("rent") || rsp.NameAr.Contains("إيجار"))), ct),
                
                // Properties by bedroom count
                PropertiesByBedrooms = await activeProperties
                    .Where(p => p.Bedrooms.HasValue)
                    .GroupBy(p => p.Bedrooms)
                    .Select(g => new BedroomCount { Bedrooms = g.Key!.Value, Count = g.Count() })
                    .OrderBy(x => x.Bedrooms)
                    .ToListAsync(ct),
                
                // Properties by type
                PropertiesByType = await (from p in activeProperties
                                         join rst in _context.RealStateTypes on p.RealStateTypeId equals rst.Id
                                         where !rst.DeletedDate.HasValue
                                         group p by new { rst.Id, rst.NameEn, rst.NameAr } into g
                                         select new PropertyTypeCount 
                                         { 
                                             TypeId = g.Key.Id,
                                             TypeName = g.Key.NameEn ?? "Unknown",
                                             TypeNameAr = g.Key.NameAr ?? "غير معروف",
                                             Count = g.Count() 
                                         })
                                         .OrderByDescending(x => x.Count)
                                         .ToListAsync(ct),
                
                // Properties by location (top 10 cities)
                PropertiesByCity = await (from p in activeProperties
                                         join c in _context.Cities on p.CityId equals c.Id
                                         where !c.DeletedDate.HasValue
                                         group p by new { c.Id, c.NameEn, c.NameAr } into g
                                         select new LocationCount 
                                         { 
                                             LocationId = g.Key.Id,
                                             LocationName = g.Key.NameEn ?? "Unknown",
                                             LocationNameAr = g.Key.NameAr ?? "غير معروف",
                                             Count = g.Count() 
                                         })
                                         .OrderByDescending(x => x.Count)
                                         .Take(10)
                                         .ToListAsync(ct),
                
                // Recent activity
                PropertiesAddedThisMonth = await activeProperties
                    .Where(p => p.InsertedDate >= DateTime.UtcNow.AddMonths(-1))
                    .CountAsync(ct),
                    
                PropertiesAddedThisWeek = await activeProperties
                    .Where(p => p.InsertedDate >= DateTime.UtcNow.AddDays(-7))
                    .CountAsync(ct),
                    
                LastUpdated = DateTime.UtcNow
            };

            await SendOkAsync(stats, ct);
        }
    }

    public class PropertyStatisticsResponse
    {
        public int TotalActiveProperties { get; set; }
        public int TotalAvailableProperties { get; set; }
        public int TotalFeaturedProperties { get; set; }
        
        public decimal AveragePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal AverageArea { get; set; }
        
        public int PropertiesForSale { get; set; }
        public int PropertiesForRent { get; set; }
        
        public List<BedroomCount> PropertiesByBedrooms { get; set; } = new();
        public List<PropertyTypeCount> PropertiesByType { get; set; } = new();
        public List<LocationCount> PropertiesByCity { get; set; } = new();
        
        public int PropertiesAddedThisMonth { get; set; }
        public int PropertiesAddedThisWeek { get; set; }
        
        public DateTime LastUpdated { get; set; }
    }

    public class BedroomCount
    {
        public int Bedrooms { get; set; }
        public int Count { get; set; }
    }

    public class PropertyTypeCount
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string TypeNameAr { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class LocationCount
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string LocationNameAr { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}