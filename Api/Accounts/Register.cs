using FastEndpoints;
using FluentValidation;
using Home.Data;
using Home.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NSwag.Annotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Home.Api.Account
{

    public class Register : Endpoint<RegisterModel,PublicResult>
    {
   
      
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/Api/Account/Register");
            AuthSchemes("ApiKeyScheme");
            
            
        }
        public async override Task HandleAsync(RegisterModel req, CancellationToken ct)
        {
            var _loc=Resolve<IStringLocalizer<Resource>>();
            var _user = Resolve<UserManager<ApplicationUser>>();
            var culture = HttpContext.Request.Headers["Accept-Language"].ToString();
            if (!string.IsNullOrEmpty(culture))
            {
                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
            }
            var user = await _user.FindByEmailAsync(req.Email);
            if (user != null)
            {
                var lan=CultureInfo.CurrentUICulture.Name;
                
                var msg = _loc["EmailRegistered"];
                await SendAsync(new PublicResult() { message = msg,status=false}, StatusCodes.Status400BadRequest);return;
            }
                user = await _user.Users.FirstOrDefaultAsync(a => a.PhoneNumber == req.Phone);


            if (user != null)
            { await SendAsync(new PublicResult() { message="there is phone number already registered"}, StatusCodes.Status400BadRequest); return; }
            ApplicationUser user1 = new ApplicationUser()
            {
                UserName = req.Email,
                Email = req.Email,
                PhoneNumber = req.Phone,
                PhoneNumberConfirmed = true,
                FirstName = req.FirstName,
                LastName = req.LastName
            };
            var fff = await _user.CreateAsync(user1, req.Password);
            if (fff.Succeeded)
            {
                await SendAsync(new PublicResult() { message="done",status=true}, StatusCodes.Status200OK);
                return;
            }
            await SendAsync(new PublicResult() { message = "couldn't register please check again", status = false }, StatusCodes.Status400BadRequest);
            return;

        }
    }


    public class RegisterModel
    {
        [AllowNull]
        public string? FirstName { get; set; }
        [AllowNull]
        public string? LastName { get; set; }
        [AllowNull]
        public string? Email { get; set; }
        [AllowNull]
        public string? Password { get; set; }
        [AllowNull]
        public string? Phone { get; set; }
    }
    public class RequestValidate : Validator<RegisterModel>
    {
        public RequestValidate(IStringLocalizer<Resource> loc)
        {
            RuleFor(a => a.FirstName).NotEmpty();
            RuleFor(a => a.LastName).NotEmpty();
            RuleFor(a=> a.Phone).NotEmpty();
            RuleFor(a=> a.Email).NotEmpty().EmailAddress().WithMessage(loc["EmailRegistered"]);
            RuleFor(a => a.Password).NotEmpty();
        }
    }
}
