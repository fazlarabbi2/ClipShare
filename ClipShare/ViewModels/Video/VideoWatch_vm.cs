using System;

namespace ClipShare.ViewModels.Video
{
    public class VideoWatch_vm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public bool IsSubscribe { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public int DisLikesCount { get; set; }
        public int ViewersCount { get; set; }
        public int SubscribersCount { get; set; }
    }
}
