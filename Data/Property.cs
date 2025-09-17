using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Data
{
    [Table("Property", Schema = "dbo")]
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? TitleAr { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(1000)]
        public string? DescriptionAr { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsNegotiable { get; set; } = false;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Area { get; set; }

        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? LivingRooms { get; set; }
        public int? Kitchens { get; set; }
        public int? FloorNumber { get; set; }
        public int? TotalFloors { get; set; }
        public int? ApartmentNumber { get; set; }
        public bool HasElevator { get; set; } = false;
        public bool HasParking { get; set; } = false;
        public int? ParkingSpaces { get; set; }
        public int? BuildYear { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(500)]
        public string? AddressDescription { get; set; }

        [MaxLength(500)]
        public string? LocationDescription { get; set; }

        // Location Foreign Keys
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }

        // Property Classification Foreign Keys
        [Required]
        public int RealStateTypeId { get; set; }
        [Required]
        public int RealStatePurposeId { get; set; }
        public int? RealStateRentTypeId { get; set; }

        // Agent/Owner Information
        [Required]
        public int AgentId { get; set; }

        [MaxLength(100)]
        public string? ContactPhone { get; set; }
        [MaxLength(100)]
        public string? ContactEmail { get; set; }

        // Property Status
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public bool IsAvailable { get; set; } = true;

        // Video and Media (excluding images - they have separate table)
        [MaxLength(500)]
        public string? VideoUrl { get; set; }

        [MaxLength(2000)]
        public string? ThreeDTour { get; set; }

        // Amenities (can be JSON or separate table)
        [MaxLength(2000)]
        public string? Amenities { get; set; }

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

        // Navigation Properties
        public virtual ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    }
}
