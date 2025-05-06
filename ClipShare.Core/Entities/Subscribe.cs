namespace ClipShare.Entities
{
    public class Subscribe
    {

        // PK (AppUserId, VideId)
        // Fk (AppUserId and FK = VieoId)
        public int AppUserId { get; set; }
        public int ChannelId { get; set; }

        // Navigations
        public AppUser AppUser { get; set; }
        public Channel Channel { get; set; }
        public ICollection<LikeDislike> LikeDislikes { get; set; }
    }
}
