using FastEndpoints;
using Home.Data;
using System.Globalization;

namespace Home.Api.Locations
{
    public class RentTypes : EndpointWithoutRequest<List<NameResponse>>
    {

        ApplicationDbContext _context;
        public RentTypes(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/General/RentTypes");
            AuthSchemes("ApiKeyScheme");
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var lan=CultureInfo.CurrentUICulture.Name;
            var abc = (from a in _context.RentTypes
                       where a.DeletedDate.HasValue == false
                       select new NameResponse{ id = a.Id, name = lan == "ar" ? a.NameAr : a.NameEn }).ToList();
            await SendAsync(abc);
        }
    }

    
}
