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

    public class Login : Endpoint<LoginModel,PublicResult>
    {
        IConfiguration _conf;
        public Login(IConfiguration conf)
        {
            _conf= conf;
        }
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/Api/Account/Login");
            AuthSchemes("ApiKeyScheme");
            //EnableAntiforgery();
            
            
        }
        public async override Task HandleAsync(LoginModel req, CancellationToken ct)
        {
            var _user = Resolve<UserManager<ApplicationUser>>();
            var _sign=Resolve<SignInManager<ApplicationUser>>();
            var user = await _user.FindByEmailAsync(req.Email);
          
            if (user == null)
            {

                await SendAsync(new PublicResult() { message="Email not Registered",status=false}, StatusCodes.Status400BadRequest);return;
            }

            var res =await _sign.PasswordSignInAsync(user, req.Password,true,true);  

          
           
            if (res.Succeeded)
            {
              var token=  JwtTokenHelper.GenerateToken(user, "Normal", _conf);
                await _user.SetAuthenticationTokenAsync(user, "jwt", "token", token);
                await SendAsync(new PublicResult() { token=token,status=true}, StatusCodes.Status200OK);
                return;
            }
            await SendAsync(new PublicResult() { message="Email or Password not correct",status=false}, StatusCodes.Status400BadRequest);
            return;

        }
    }
    public class CheckLogin : EndpointWithoutRequest<string>
    {
        
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/Api/Account/CheckLogin");
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            //EnableAntiforgery();


        }
        public async override Task HandleAsync(CancellationToken ct)
        {
            
            await SendAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            

        }
    }


    public class LoginModel
    {
        
        public string Email { get; set; }
        public string Password { get; set; }
       
    }
  

    public class LoginRequestValidate : Validator<LoginRequest>
    {
        public LoginRequestValidate()
        {
         
            RuleFor(a=> a.Email).NotEmpty().EmailAddress();
            RuleFor(a => a.Password).NotEmpty();
        }
    }
}
