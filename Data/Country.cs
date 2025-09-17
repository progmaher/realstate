using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Data
{
    [Table("Country", Schema = "dbo")]
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public DateTime? InsertedDate { get; set; }
        public string? InsertedBy { get; set; }
        public DateTime? UPdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<City>? Cities { get; set; }
        public ICollection<District>? Districts { get; set; }
    }

}
