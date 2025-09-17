using FastEndpoints;
using Home.Data;
using System.Globalization;

namespace Home.Api.Locations
{
    
    public class Country : EndpointWithoutRequest<List<NameResponse>>
    {

        ApplicationDbContext _context;
        public Country(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/Locations/Countries");
            AuthSchemes("ApiKeyScheme");
            Summary(s =>
            {
                
                s.Summary = "Get all Countries";
                s.Description = "Get all Countries";
            });
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var lan = CultureInfo.CurrentCulture.Name;
            
            var abc = (from a in _context.Countries
                       where a.DeletedDate.HasValue == false

                       select new NameResponse{ id = a.Id, name = lan == "ar" ? a.NameAr : a.NameEn }).ToList();
            await SendAsync(abc);
        }
    }

    
}
