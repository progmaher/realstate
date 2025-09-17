using FastEndpoints;
using FluentValidation;
using Home.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Home.Api.Account
{

    public class UpdateProfilePhoto : Endpoint<ProfilePhoto,string>
    {
        IConfiguration _conf;
        public UpdateProfilePhoto(IConfiguration conf)
        {
            _conf= conf;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/Api/Account/UpdateProfilePhoto");
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            AllowFileUploads();
            //EnableAntiforgery();


        }
        public async override Task HandleAsync(ProfilePhoto? req, CancellationToken ct)
        {
            var _user = Resolve<UserManager<ApplicationUser>>();
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _user.FindByIdAsync(userid);
          
            if (user == null)
            {

                await SendAsync("no user", StatusCodes.Status400BadRequest);return;
              
            }

          if(req.Photo!=null)
          {
                var dd = DateTime.UtcNow;
            var    photo = $"Home{dd.Year}{dd.Month}{dd.Day}{dd.Hour}{dd.Minute}{dd.Second}{Path.GetExtension(req.Photo.FileName)}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", photo);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    req.Photo.CopyTo(stream);
                }
               user.Photo = photo;
            
            }
            user.FirstName= req.FirstName;
            user.LastName= req.LastName;
            await _user.UpdateAsync(user);
            var token = JwtTokenHelper.GenerateToken(user, "User", _conf);
            await SendAsync(token, StatusCodes.Status200OK);
            return;

        }
    }
    
    public class ProfilePhoto
    {
        [AllowNull]
        public IFormFile? Photo { get; set; }
        [AllowNull]
        public string? FirstName { get; set; }
        [AllowNull]
        public string? LastName { get; set; }
    }
    

}
