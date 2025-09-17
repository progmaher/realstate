using FastEndpoints;
using FastEndpoints.Swagger;
using Home.Data;
using Home.Services;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.text", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddRazorPages();
  
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
              ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    }).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>("ApiKeyScheme", null);

builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints().SwaggerDocument();
//builder.Services.AddLocalization(options=> options.ResourcesPath= "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "ar" };
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);

    // تفعيل قراءة اللغة من الهيدر Accept-Language
    options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
    options.ApplyCurrentCultureToResponseHeaders = true;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add background service for image processing
builder.Services.AddHostedService<BackgroundImageProcessingService>();

// Register ImageSharp dependencies
builder.Services.AddSingleton<SixLabors.ImageSharp.Configuration>(SixLabors.ImageSharp.Configuration.Default);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  //  app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication(); // Don't forget this
app.UseAuthorization();
app.UseFastEndpoints().UseSwaggerGen();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.MapRazorPages();

// Create tables if they don't exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var sqlHelper = new SqlHelper(builder.Configuration);
        
        // First create Property table
        var propertyTableResult = await sqlHelper.CreatePropertyTableAsync();
        if (propertyTableResult)
        {
            Console.WriteLine("Property table check completed.");
            
            // Then create PropertyImage table (which depends on Property table)
            var propertyImageTableResult = await sqlHelper.CreatePropertyImageTableAsync();
            if (propertyImageTableResult)
            {
                Console.WriteLine("PropertyImage table check completed.");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database tables.");
    }
}

app.Run();
 