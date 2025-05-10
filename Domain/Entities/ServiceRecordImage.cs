using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceLog.Domain.Entities
{
    public class ServiceRecordImage : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(ServiceRecord))]
        public int ServiceRecordId { get; set; }
        [Required]
        public required string ImagePath { get; set; }
        public ServiceRecord ServiceRecord { get; set; } = null!;
    }
}
