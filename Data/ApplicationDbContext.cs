using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Home.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<AgentBranch> AgentBranches { get; set; }
        public virtual DbSet<AgentManager> AgentManagers { get; set; }
        public virtual DbSet<AgentDocument> AgentDocuments { get; set; }
        public virtual DbSet<RealStateRentType> RentTypes { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<RealStatePurpose> RealStatePurposes { get; set; }
        public virtual DbSet<RealStateType> RealStateTypes { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyImage> PropertyImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Property relationships to avoid cascade delete conflicts
            modelBuilder.Entity<Property>()
                .HasOne<Country>()
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Property>()
                .HasOne<City>()
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Property>()
                .HasOne<District>()
                .WithMany()
                .HasForeignKey(p => p.DistrictId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Property>()
                .HasOne<Agent>()
                .WithMany()
                .HasForeignKey(p => p.AgentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Property>()
                .HasOne<RealStateType>()
                .WithMany()
                .HasForeignKey(p => p.RealStateTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Property>()
                .HasOne<RealStatePurpose>()
                .WithMany()
                .HasForeignKey(p => p.RealStatePurposeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Property>()
                .HasOne<RealStateRentType>()
                .WithMany()
                .HasForeignKey(p => p.RealStateRentTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure District relationships
            modelBuilder.Entity<District>()
                .HasOne<Country>()
                .WithMany()
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<District>()
                .HasOne<City>()
                .WithMany()
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure PropertyImage relationships
            modelBuilder.Entity<PropertyImage>()
                .HasOne<Property>()
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
