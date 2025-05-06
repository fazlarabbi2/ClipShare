using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClipShare.Entities
{
    public class AppUser : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public AppUser AppUsers { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Subscribe> Subscriptions { get; set; }
        public ICollection<LikeDislike> LikeDislikes { get; set; }
    }
}
