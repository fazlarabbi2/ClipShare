using ClipShare.Core.DTOs;
using ClipShare.Core.IRepo;
using ClipShare.Core.Pagination;
using ClipShare.Entities;
using ClipShare.Utility;
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

        public async Task<PaginatedList<VideoGridChannelDto>> GetVideosForChannelGrid(int channelId, BaseParameters parameters)
        {
            var query = _context.Video
                .Include(x => x.Category)
                .Where(x => x.ChannelId == channelId)
                .Select(x => new VideoGridChannelDto
                {
                    Id = x.Id,
                    ThumbnailURL = x.ThumbnailUrl,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                    CategoryName = x.Category.Name,
                    Views = SD.GetRandomNumber(1000, 5000, x.Id),
                    Comments = SD.GetRandomNumber(1, 100, x.Id),
                    Likes = SD.GetRandomNumber(10, 100, x.Id),
                    Dislikes = SD.GetRandomNumber(5, 100, x.Id)
                })
                .AsQueryable();

            query = parameters.SortBy switch
            {
                "title-a"=>query.OrderBy(x=>x.Title),
                "title-d" => query.OrderByDescending(x=>x.Title),
                 "date-a" => query.OrderBy(u=>u.CreatedAt),
                 "date-d" => query.OrderByDescending(u=>u.CreatedAt),
                 "views-a" => query.OrderBy(u=>u.Views.ToString()),
                 "views-d" => query.OrderByDescending(u=>u.Views.ToString()),
                 "comments-a" => query.OrderBy(u=>u.Comments),
                 "comments-d" => query.OrderByDescending(u=>u.Comments),
                 "lieks-a" => query.OrderBy(u=>u.Likes),
            }
        }
    }
}
