using FastEndpoints;
using FluentValidation;

namespace Home.Api.Agent
{
    public class PropertyImageRequest
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string? ImageTitle { get; set; }
        public string? ImageDescription { get; set; }
        public bool IsMainImage { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
        public string? ImageType { get; set; } // "interior", "exterior", "floor_plan", etc.
    }
    public class AddPropertyRequest
    {
        // العنوان عربي وإنجليزي
        public string Title { get; set; } = string.Empty;
        public string? TitleAr { get; set; }
        
        // الوصف عربي وإنجليزي  
        public string? Description { get; set; }
        public string? DescriptionAr { get; set; }
        
        // السعر وقابلية التفاوض
        public decimal Price { get; set; }
        public bool IsNegotiable { get; set; } = false;
        
        // المساحة
        public decimal? Area { get; set; }
        
        // تفاصيل العقار
        public int? Bedrooms { get; set; }          // عدد غرف النوم
        public int? Bathrooms { get; set; }         // عدد دورات المياه
        public int? LivingRooms { get; set; }       // عدد الصالات
        public int? Kitchens { get; set; }          // عدد المطابخ
        public int? FloorNumber { get; set; }       // رقم الطابق
        public int? TotalFloors { get; set; }       // عدد الطوابق
        public int? ApartmentNumber { get; set; }   // رقم الشقة
        public bool HasElevator { get; set; } = false;  // هل يوجد مصعد
        public bool HasParking { get; set; } = false;   // هل يوجد موقف
        public int? ParkingSpaces { get; set; }     // عدد مواقف السيارات
        public int? BuildYear { get; set; }
        
        // العنوان والموقع
        public string? Address { get; set; }
        public string? AddressDescription { get; set; }  // وصف العنوان
        public string? LocationDescription { get; set; } // اللوكيشن
        
        // Location IDs - الدولة والمدينة والحي
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        
        // Property Classification - نوع العقار وطبيعة الإعلان والهدف
        public int RealStateTypeId { get; set; }      // نوع العقار
        public int RealStatePurposeId { get; set; }   // طبيعة الإعلان
        public int? RealStateRentTypeId { get; set; } // الهدف من العقار
        
        // Contact Information
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        
        // الصور والوسائط
        public List<PropertyImageRequest>? Images { get; set; }  // قائمة الصور
        public string? VideoUrl { get; set; }         // فيديو
        public string? ThreeDTour { get; set; }       // ثلاثي أبعاد
        
        // Property Features
        public bool IsFeatured { get; set; } = false;
        public List<string>? Amenities { get; set; }
    }

    public class AddPropertyValidator : Validator<AddPropertyRequest>
    {
        public AddPropertyValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Property title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.TitleAr)
                .MaximumLength(200).WithMessage("Arabic title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.DescriptionAr)
                .MaximumLength(1000).WithMessage("Arabic description cannot exceed 1000 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Area)
                .GreaterThan(0).When(x => x.Area.HasValue)
                .WithMessage("Area must be greater than 0 if provided");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("Country is required");

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("City is required");

            RuleFor(x => x.DistrictId)
                .GreaterThan(0).WithMessage("District is required");

            RuleFor(x => x.RealStateTypeId)
                .GreaterThan(0).WithMessage("Property type is required");

            RuleFor(x => x.RealStatePurposeId)
                .GreaterThan(0).WithMessage("Property purpose is required");

            RuleFor(x => x.ContactPhone)
                .Matches(@"^[\+]?[1-9][\d]{0,15}$")
                .When(x => !string.IsNullOrEmpty(x.ContactPhone))
                .WithMessage("Invalid phone number format");

            RuleFor(x => x.ContactEmail)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.ContactEmail))
                .WithMessage("Invalid email format");

            RuleFor(x => x.Bedrooms)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Bedrooms.HasValue)
                .WithMessage("Bedrooms cannot be negative");

            RuleFor(x => x.Bathrooms)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Bathrooms.HasValue)
                .WithMessage("Bathrooms cannot be negative");

            RuleFor(x => x.LivingRooms)
                .GreaterThanOrEqualTo(0)
                .When(x => x.LivingRooms.HasValue)
                .WithMessage("Living rooms cannot be negative");

            RuleFor(x => x.Kitchens)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Kitchens.HasValue)
                .WithMessage("Kitchens cannot be negative");

            RuleFor(x => x.FloorNumber)
                .GreaterThanOrEqualTo(0)
                .When(x => x.FloorNumber.HasValue)
                .WithMessage("Floor number cannot be negative");

            RuleFor(x => x.TotalFloors)
                .GreaterThan(0)
                .When(x => x.TotalFloors.HasValue)
                .WithMessage("Total floors must be greater than 0");

            RuleFor(x => x.ApartmentNumber)
                .GreaterThan(0)
                .When(x => x.ApartmentNumber.HasValue)
                .WithMessage("Apartment number must be greater than 0");

            RuleFor(x => x.ParkingSpaces)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ParkingSpaces.HasValue)
                .WithMessage("Parking spaces cannot be negative");

            RuleFor(x => x.BuildYear)
                .GreaterThan(1800).LessThanOrEqualTo(DateTime.Now.Year + 5)
                .When(x => x.BuildYear.HasValue)
                .WithMessage("Build year must be reasonable");

            // Validate Images
            RuleForEach(x => x.Images)
                .SetValidator(new PropertyImageValidator())
                .When(x => x.Images != null);

            // Ensure only one main image
            RuleFor(x => x.Images)
                .Must(images => images == null || images.Count(img => img.IsMainImage) <= 1)
                .WithMessage("Only one image can be set as main image");
        }
    }

    public class PropertyImageValidator : Validator<PropertyImageRequest>
    {
        public PropertyImageValidator()
        {
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required")
                .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters");

            RuleFor(x => x.ImageTitle)
                .MaximumLength(200).WithMessage("Image title cannot exceed 200 characters");

            RuleFor(x => x.ImageDescription)
                .MaximumLength(500).WithMessage("Image description cannot exceed 500 characters");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order cannot be negative");

            RuleFor(x => x.ImageType)
                .MaximumLength(100).WithMessage("Image type cannot exceed 100 characters");
        }
    }
}
