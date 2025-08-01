using ClipShare.Core.DTOs;
using ClipShare.Core.Pagination;
using ClipShare.Entities;

namespace ClipShare.Core.IRepo
{
    public interface IVideoRepo : IBaseRepo<Video>
    {
        Task<int> GetUserIdByVideoId(int videoId);
        Task<PaginatedList<VideoGridChannelDto>> GetVideosForChannelGridAsync(int channelId, BaseParameters parameters);
        Task<PaginatedList<VideoForHomeGridDto>> GetVideosForHomeGridAsync(HomeParameters parameters);
    }
}
