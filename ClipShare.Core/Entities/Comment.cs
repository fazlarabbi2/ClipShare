using System.ComponentModel.DataAnnotations;

namespace ClipShare.Entities
{
    public class Comment
    {
        // PK (AppUserId, VideId)
        // Fk (AppUserId and FK = VieoId)
        public int AppUserId { get; set; }
        public int VideoId { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime PostedAt { get; set; }

        // Navigation properties
        public AppUser AppUser { get; set; }
        public Video Video { get; set; }
    }
}
