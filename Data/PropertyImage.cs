using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Data
{
    [Table("PropertyImage", Schema = "dbo")]
    public class PropertyImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ThumbnailUrl { get; set; }

        [MaxLength(200)]
        public string? ImageTitle { get; set; }

        [MaxLength(500)]
        public string? ImageDescription { get; set; }

        public bool IsMainImage { get; set; } = false;

        public int DisplayOrder { get; set; } = 0;

        [MaxLength(100)]
        public string? ImageType { get; set; } // "interior", "exterior", "floor_plan", etc.
        
        public DateTime? ProcessedDate { get; set; }

        // Audit Fields
        public DateTime InsertedDate { get; set; } = DateTime.UtcNow;
        [MaxLength(100)]
        public string? InsertedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        [MaxLength(100)]
        public string? DeletedBy { get; set; }
    }
}
