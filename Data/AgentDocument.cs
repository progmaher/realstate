using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Home.Data
{
    [Table("AgentDocument")]
    public class AgentDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int AgentId { get; set; }
        public int DocumentTypeId { get; set; }
        public string? FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        public Agent? Agent { get; set; }
        public DocumentType? DocumentType { get; set; }
    }

}
