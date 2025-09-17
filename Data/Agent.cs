using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Data
{
    [Table("Agent")]
    public class Agent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string CR { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpiryDate { get; set; }
        public string? Website { get; set; }
        public string? LogoPath { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AgentBranch>? Branches { get; set; }
        public ICollection<AgentManager>? Managers { get; set; }
        public ICollection<AgentDocument>? Documents { get; set; }
    }

}
