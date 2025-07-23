namespace ClipShare.Entities
{
    public class LikeDislike
    {
        // PK (AppUserId, VideId)
        // Fk (AppUserId and FK = VieoId)
        public int AppUserId { get; set; }
        public int VideoId { get; set; }
        public bool Liked { get; set; }

        // Navigations
        public AppUser AppUser { get; set; }
        public Video Video { get; set; }
    }
}
