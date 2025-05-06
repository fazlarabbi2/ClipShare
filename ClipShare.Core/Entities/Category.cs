using System.ComponentModel.DataAnnotations;

namespace ClipShare.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Video> Videos { get; set; }
    }
}
