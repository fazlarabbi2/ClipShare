using ClipShare.Core.IRepo;
using ClipShare.Entities;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace ClipShare.DataAccess.Repo
{
    public class ChannelRepo : BaseRepo<Channel>, IChannelRepo
    {
        private readonly Context _context;

        public ChannelRepo(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetChannelIdByUserId(int userId)
        {
            return await _context.Channel.Where(x => x.AppUserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
