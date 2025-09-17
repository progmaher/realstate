namespace Home.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? AccountType { get; set; } // مثال: Agent, Manager, Admin, etc.

        public int AccountId { get; set; } // المفتاح الخارجي حسب نوع الحساب المرتبط (AgentId أو ManagerId مثلاً)

        public string? UserId { get; set; } // ربما تقصد UserId؟ إذا لا، نحتفظ به كما هو

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLogin { get; set; }
    }

}
