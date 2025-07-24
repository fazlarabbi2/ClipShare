using ClipShare.Entities;

namespace ClipShare.Core.IRepo
{
    public interface IVideoRepo : IBaseRepo<Video>
    {
        Task<int> GetUserIdByVideoId(int videoId);
    }
}
