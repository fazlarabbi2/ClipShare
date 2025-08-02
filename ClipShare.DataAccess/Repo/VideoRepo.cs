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

        public async Task<PaginatedList<VideoGridChannelDto>> GetVideosForChannelGridAsync(int channelId, BaseParameters parameters)
        {
            var query = _context.Video
                .Include(x => x.Category)
                .Where(x => x.ChannelId == channelId)
                .Select(x => new VideoGridChannelDto
                {
                    Id = x.Id,
                    ThumbnailUrl = x.ThumbnailUrl,
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
                "title-a" => query.OrderBy(x => x.Title),
                "title-d" => query.OrderByDescending(x => x.Title),
                "date-a" => query.OrderBy(u => u.CreatedAt),
                "date-d" => query.OrderByDescending(u => u.CreatedAt),
                "views-a" => query.OrderBy(u => u.Views),
                "views-d" => query.OrderByDescending(u => u.Views),
                "comments-a" => query.OrderBy(u => u.Comments),
                "comments-d" => query.OrderByDescending(u => u.Comments),
                "likes-a" => query.OrderBy(u => u.Likes),
                "likes-d" => query.OrderByDescending(u => u.Likes),
                "dislikes-a" => query.OrderBy(u => u.Dislikes),
                "dislikes-d" => query.OrderByDescending(u => u.Dislikes),
                "category-a" => query.OrderBy(u => u.CategoryName),
                "category-d" => query.OrderByDescending(u => u.CategoryName),
                _ => query.OrderByDescending(u => u.CreatedAt)
            };

            return await PaginatedList<VideoGridChannelDto>.CreateAsync(query.AsNoTracking(), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PaginatedList<VideoForHomeGridDto>> GetVideosForHomeGridAsync(HomeParameters parameters)
        {
            var query = _context.Video
                .Select(x => new VideoForHomeGridDto
                {
                    Id = x.Id,
                    ThumbnailUrl = x.ThumbnailUrl,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    ChannelId = x.ChannelId,
                    ChannelName = x.Channel.Name,
                    CategoryId = x.CategoryId,
                    Views = SD.GetRandomNumber(100, 50000, x.Id)
                }).AsQueryable();

            if (parameters.CategoryId > 0)
            {
                query = query.Where(x => x.CategoryId == parameters.CategoryId);
            }
            if (!string.IsNullOrEmpty(parameters.SearchBy))
            {
                query = query.Where(x => x.Title.ToLower().Contains(parameters.SearchBy) || x.Description.ToLower().Contains(parameters.SearchBy));
            }

            return await PaginatedList<VideoForHomeGridDto>.CreateAsync(query.AsNoTracking(), parameters.PageNumber, parameters.PageSize);
        }
    }
}
