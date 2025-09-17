using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Home.Data
{
    [Table("agentBranch")]
    public class AgentBranch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AgentId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string? AddressAr { get; set; }
        public string? AddressEn { get; set; }
        public string? ShortAddress { get; set; }
        public int? BuildingNo { get; set; }
        public int? AdditonalNo { get; set; }
        public int? ZipeCode { get; set; }
        public string? LandlinePhone { get; set; }
        public string? MobilePhone { get; set; }
        public string? Email { get; set; }
        public string? WhatsApp { get; set; }
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; }

        public Agent? Agent { get; set; }
        public Country? Country { get; set; }
        public City? City { get; set; }
        public District? District { get; set; }
    }

    public class AgentBranchModel
    {
        [AllowNull]
        public int? Id { get; set; }
        [AllowNull]
        public int? AgentId { get; set; }
        [AllowNull]
        public string? BranchName { get; set; } = string.Empty;
        [AllowNull]
        public int? CountryId { get; set; }
        [AllowNull]
        public int? CityId { get; set; }
        [AllowNull]
        public int? DistrictId { get; set; }
        [AllowNull]
        public string? AddressAr { get; set; }
        [AllowNull]
        public string? AddressEn { get; set; }
        [AllowNull]
        public string? ShortAddress { get; set; }
        [AllowNull]
        public int? BuildingNo { get; set; }
        [AllowNull]
        public int? AdditonalNo { get; set; }
        [AllowNull]
        public int? ZipeCode { get; set; }
        [AllowNull]
        public string? LandlinePhone { get; set; }
        [AllowNull]
        public string? MobilePhone { get; set; }
        [AllowNull]
        public string? Email { get; set; }
        [AllowNull]
        public string? WhatsApp { get; set; }
        [AllowNull]
        public bool IsMain { get; set; }

    }

}
