namespace ClipShare.Entities
{
    public class Channel : BaseEntity
    {
        public string Name { get; set; }
        public string About { get; set; }
        public string CreatedAt { get; set; }
        public int AppUserId { get; set; }

        // Navigations
        public AppUser AppUser { get; set; }
        public ICollection<Video> Videos { get; set; }

        public ICollection<Subscribe> Subscribers { get; set; }
    }
}
