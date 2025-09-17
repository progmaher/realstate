using FastEndpoints;
using Home.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Home.Api.Locations
{
    public class DocumentTypes : EndpointWithoutRequest<List<NameResponse>>
    {

        ApplicationDbContext _context;
        public DocumentTypes(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/Locations/Nationalities");
            AuthSchemes("ApiKeyScheme");
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var lan = CultureInfo.CurrentCulture.Name;
            var data = await _context.Nationalities
         .Where(a => !a.DeletedDate.HasValue)
         .Select(a => new NameResponse
         {
             id = a.Id,
             name = lan == "ar" ? a.NameAr : a.NameEn
         })
         .ToListAsync(ct);

            await SendAsync(data ?? new List<NameResponse>(), cancellation: ct, statusCode: 200);
        }
    }

    
}
