using ClipShare.Entities;

namespace ClipShare.Core.IRepo
{
    public interface IChannelRepo : IBaseRepo<Channel>
    {
        Task<int> GetChannelIdByUserId(int userId);
    }
}
