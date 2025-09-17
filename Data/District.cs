using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Data
{
    [Table("District", Schema = "dbo")]
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public DateTime? InsertedDate { get; set; }
        public string? InsertedBy { get; set; }
        public DateTime? UPdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }

}
