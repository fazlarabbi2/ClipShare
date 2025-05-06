using System.ComponentModel.DataAnnotations;

namespace ClipShare.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
