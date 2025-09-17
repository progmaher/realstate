using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Data
{
    [Table("RealStateType")]
    public class RealStateType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameAr { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NameEn { get; set; } = string.Empty;

        public DateTime InsertedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? InsertedBy { get; set; }

        public DateTime? UPdatedDate { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        [MaxLength(100)]
        public string? DeletedBy { get; set; }
    }
}
