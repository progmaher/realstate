using FastEndpoints;
using FluentValidation;
using Home.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NSwag.Annotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using static FastEndpoints.Ep;

namespace Home.Api.Agent
{

    public class RegisterAgent : Endpoint<RegisterAgentModel, PublicResult>
    {
        ILogger<RegisterAgent> _logger;
        ApplicationDbContext _context;
        public RegisterAgent(ApplicationDbContext context, ILogger<RegisterAgent> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/Api/Agent/Register");
            AuthSchemes("ApiKeyScheme");
            AllowFileUploads();

        }
        public async override Task HandleAsync(RegisterAgentModel req, CancellationToken ct)
        {
            _logger.LogInformation("Req Date {date}", DateTime.Now);
            _logger.LogInformation("REQ : {req}", JsonConvert.SerializeObject(req));
            var _user = Resolve<UserManager<ApplicationUser>>();
            var user = new ApplicationUser();
            var agent = new Data.Agent();

         if (req.Register==null)
         {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                PublicResult result =await createagent(req, userid, _user);
                await SendAsync(result, (result.status == true ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest));
                return;
         }
            else
            {
                user =await _user.FindByEmailAsync(req.Register.Email);
                if (user != null)
                {
                    await SendAsync(new PublicResult() { message = "this email already registered", status = false }, StatusCodes.Status400BadRequest); return;
                }
                user = await _user.Users.FirstOrDefaultAsync(a => a.PhoneNumber == req.Register.Phone);
                if (user != null)
                { await SendAsync(new PublicResult() { message = "there is phone number already registered" }, StatusCodes.Status400BadRequest); return; }
                ApplicationUser user1 = new ApplicationUser()
                {
                    UserName = req.Register.Email,
                    Email = req.Register.Email,
                    PhoneNumber = req.Register.Phone,
                    PhoneNumberConfirmed = true,
                    FirstName = req.Register.FirstName,
                    LastName = req.Register.LastName
                };
                var fff = await _user.CreateAsync(user1, req.Register.Password);
                if (fff.Succeeded)
                {
                    PublicResult result = await createagent(req, user1.Id, _user);
                    await SendAsync(result, (result.status == true ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest));
                    return;
                }
                await SendAsync(new PublicResult() { message = "couldn't register please check again", status = false }, StatusCodes.Status400BadRequest);
                return;
            }



         

        }
       async Task<PublicResult> createagent(RegisterAgentModel req,string userid,UserManager<ApplicationUser> _user)
        {
            try
            {
                var cccc = JsonConvert.SerializeObject(req);
                _logger.LogInformation("Saving Agent");
                _logger.LogInformation($"req {cccc}");
                var agent = _context.Agents.FirstOrDefault(a => a.LicenseNumber == req.FAL || a.CR == req.CR);
                if (agent != null)
                {
                    return new PublicResult() { message = "there is an account by same FAL or CR", status = false };
                }

                agent = new Data.Agent()
                {
                    CR = req.CR,
                    LicenseNumber = req.FAL,
                    CreatedAt = DateTime.UtcNow,
                    LicenseExpiryDate = DateTime.Parse(req.FALExpiryDate),
                    NameAr = req.NameAr,
                    NameEn = req.NameEn,
                    IsActive = false
                };
                if (req.Logo != null)
                {
                    var dd = DateTime.UtcNow;
                    var photo = $"Home{dd.Year}{dd.Month}{dd.Day}{dd.Hour}{dd.Minute}{dd.Second}{Path.GetExtension(req.Logo.FileName)}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Agents", photo);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        req.Logo.CopyTo(stream);
                    }
                    agent.LogoPath = photo;

                }
                _context.Agents.Add(agent);
                await _context.SaveChangesAsync();


                _logger.LogInformation("Saving Account");

                Data.Account account = new Data.Account()
                {
                    AccountType = "Agent",
                    AccountId = agent.Id,
                    IsActive = true,
                    LastLogin = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UserId = userid
                };
                
                _context.Accounts.Add(account);
                _context.SaveChanges();
                _logger.LogInformation("Saving Branch");
                if (req.agentBranch != null)
                {
                    Data.AgentBranch agentBranch = new Data.AgentBranch()
                    {
                        CountryId = req.agentBranch.CountryId,
                        CityId = req.agentBranch.CityId,
                        DistrictId = req.agentBranch.DistrictId,
                        CreatedAt = DateTime.UtcNow,
                        BranchName = req.agentBranch.BranchName,
                        AgentId = agent.Id,
                        AddressAr = req.agentBranch.AddressAr,
                        AddressEn = req.agentBranch.AddressEn,
                        AdditonalNo = req.agentBranch.AdditonalNo,
                        BuildingNo = req.agentBranch.BuildingNo,
                        Email = req.agentBranch.Email,
                        IsMain = req.agentBranch.IsMain,
                        LandlinePhone = req.agentBranch.LandlinePhone,
                        MobilePhone = req.agentBranch.MobilePhone,
                        ShortAddress = req.agentBranch.ShortAddress,
                        WhatsApp = req.agentBranch.WhatsApp,
                        ZipeCode = req.agentBranch.ZipeCode
                    };
                    _context.AgentBranches.Add(agentBranch);
                    _context.SaveChanges();
                }
                return new PublicResult() { message = "Under Review", status = true }; ;
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"req error", ex.Message);
                return new PublicResult() { message = ex.Message, status = false }; 
            }
          
        }
    }


    public class RegisterAgentModel
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string CR { get; set; }
        public string FAL { get; set; }
        public string FALExpiryDate {  get; set; }
        [AllowNull]
        public IFormFile? Logo   { get; set; }
       
        [AllowNull]
       public Account.RegisterModel? Register { get; set; }
        [AllowNull]
        public AgentBranchModel? agentBranch { get; set; }

    }
    public class RegisterAgentModelvalidator : Validator<RegisterAgentModel>
    {
        public RegisterAgentModelvalidator()
        {
            RuleFor(a => a.NameAr).NotEmpty();
            RuleFor(a => a.NameEn).NotEmpty();
            RuleFor(a=> a.FAL).NotEmpty();
            RuleFor(a=> a.FALExpiryDate).NotEmpty();
            RuleFor(a => a.CR).NotEmpty();
            //RuleFor(a => a.agentBranch.CountryId).NotEmpty().NotNull();
            //RuleFor(a => a.agentBranch.CityId).NotEmpty().NotNull();
            //RuleFor(a => a.agentBranch.DistrictId).NotEmpty().NotNull();

            //  RuleFor(a => a.Logo).NotEmpty();
        }
    }
}
