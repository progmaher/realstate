using FastEndpoints;
using Home.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Home.Api.Agent
{
    public class Profile :EndpointWithoutRequest<object>
    {
        ILogger<Profile> _logger;
        ApplicationDbContext _context;
        public Profile(ApplicationDbContext context,ILogger<Profile> logger)
        {
            _context = context;
            _logger = logger;
        }
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/Agent/Profile");
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var account =await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userid);
            if (account != null)
            {
                var agent = await _context.Agents.FindAsync(account.AccountId) ;
                if (agent != null) 
                {
                    var afv=await _context.AgentBranches.Where(a=> a.AgentId==agent.Id).ToListAsync() ;
                    agent.Branches= afv ;   
                    await SendAsync(new { data=agent,status=true},StatusCodes.Status200OK);
                    return;
                }
                await SendAsync(new {message="you are no",status=false}, StatusCodes.Status200OK);
                return;
            }

        }
    }
}
