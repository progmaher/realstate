using FastEndpoints;
using Home.Data;
using System.Globalization;

namespace Home.Api.Locations
{
    public class District : Endpoint<LanIdRequest,List<NameResponse>>
    {

        ApplicationDbContext _context;
        public District(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/Locations/Districts");
            AuthSchemes("ApiKeyScheme");
        }
        public override async Task HandleAsync(LanIdRequest req,CancellationToken ct)
        {
            var lan = CultureInfo.CurrentCulture.Name;
            var abc = (from a in _context.Districts
                       where a.DeletedDate.HasValue == false && a.CityId==req.id
                       select new NameResponse{ id = a.Id, name = lan == "ar" ? a.NameAr : a.NameEn }).ToList();
            await SendAsync(abc);
        }
    }

    
}
