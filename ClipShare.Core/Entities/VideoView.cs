using ClipShare.Entities;

namespace ClipShare.Core.Entities
{
    public class VideoView
    {
        // Bridge table between AppUser and Video
        // PK (AppUserId, VideoId)
        // FK = AppUserId and FK = VideoId
        public int AppUserId { get; set; }
        public int VideoId { get; set; }

        // IP2Location
        public string IpAddress { get; set; }
        public int NumberOfVisit { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool Is_Proxy { get; set; }
        public DateTime LastVisit { get; set; } = DateTime.UtcNow;

        //Navigations
        public AppUser AppUser { get; set; }
        public Video Video { get; set; }
    }
}
