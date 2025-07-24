using ClipShare.Core.IRepo;
using ClipShare.Entities;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace ClipShare.DataAccess.Repo
{
    public class VideoRepo(Context _context) : BaseRepo<Video>(_context), IVideoRepo
    {
        public async Task<int> GetUserIdByVideoId(int videoId)
        {
            return await _context.Video
                .Where(x => x.Id == videoId)
                .Select(s => s.Channel.AppUserId)
                .FirstOrDefaultAsync();
        }
    }

}
