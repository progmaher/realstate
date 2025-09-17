using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Data
{
    [Table("AgentManagers")]
    public class AgentManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ManagerId { get; set; }
        public int AgentId { get; set; }
        public int? BranchId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int? NationalId { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }

        public Agent? Agent { get; set; }
        public AgentBranch? Branch { get; set; }
    }

}
