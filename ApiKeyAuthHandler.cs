using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

public class ApiKeyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _config;

    public ApiKeyAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
 
        IConfiguration config)
        : base(options, logger, encoder)        
    {
        _config = config;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("apikey", out var extractedApiKey))
            return Task.FromResult(AuthenticateResult.Fail("API Key not provided"));

        var configuredApiKey = _config["ApiKeySettings:Key"];

        if (extractedApiKey != configuredApiKey)
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));

        var claims = new[] {
            new Claim(ClaimTypes.Name, "APIUser")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

  