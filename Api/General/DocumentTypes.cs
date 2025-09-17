using FastEndpoints;
using Home.Data;

namespace Home.Api.General
{
    public class DocumentTypes : Endpoint<LanRequest,List<NameResponse>>
    {

        ApplicationDbContext _context;
        public DocumentTypes(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/General/DocumentTypes");
            AuthSchemes("ApiKeyScheme");
        }
        public override async Task HandleAsync(LanRequest lan,CancellationToken ct)
        {
            var abc = (from a in _context.DocumentTypes
                       where a.DeletedDate.HasValue == false
                       select new NameResponse{ id = a.Id, name = lan.lan == "ar" ? a.NameAr : a.NameEn }).ToList();
            await SendAsync(abc);
        }
    }

    
}
