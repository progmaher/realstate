using FastEndpoints;
using FluentValidation;
using Home.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Home.Api.Account
{

    public class ChangePassword : Endpoint<ChangePasswordModel,PublicResult>
    {
        IConfiguration _conf;
        public ChangePassword(IConfiguration conf)
        {
            _conf= conf;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/Api/Account/ChangePassword");
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            AllowFileUploads();
            //EnableAntiforgery();


        }
        public async override Task HandleAsync(ChangePasswordModel? req, CancellationToken ct)
        {
            var _user = Resolve<UserManager<ApplicationUser>>();
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _user.FindByIdAsync(userid);
          
            if (user == null)
            {

                await SendAsync(new PublicResult() { message = "no user or login again", status = false }, StatusCodes.Status400BadRequest);return;
                return;
            }

         var result=  await _user.ChangePasswordAsync(user,req.CurrentPassword,req.NewPassword);
            if(result.Succeeded)
            {
                await SendAsync(new PublicResult() { message = "Done" ,status=true}, StatusCodes.Status200OK);
                return;

            }
            else
            {
                await SendAsync(new PublicResult() { message = "current password not correct",status=false }, StatusCodes.Status200OK);
                return;

            }


        }
    }
    
    public class ChangePasswordModel
    {
        [AllowNull]
        [PasswordPropertyText]
        public string? CurrentPassword { get; set; }
        [AllowNull]
        [PasswordPropertyText]
        public string? NewPassword { get; set; }
    }
    

}
