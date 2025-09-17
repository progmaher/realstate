using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Home.Data
{
    public class ApplicationUser : IdentityUser
    {
        [AllowNull]
        public string? FirstName { get; set; }
        [AllowNull]
        public string? LastName { get; set; }
        [AllowNull]
        public string? Photo {  get; set; }
    }
}
