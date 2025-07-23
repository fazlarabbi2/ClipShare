using ClipShare.Core.IRepo;
using DataAccess.Data;

namespace ClipShare.DataAccess.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

        public IChannelRepo ChannelRepo => new ChannelRepo(_context);

        public ICategoryRepo CategoryRepo => new CategoryRepo(_context);

        public IVideoRepo VideoRepo => new VideoRepo(_context);

        public async Task<bool> CompleteAsync()
        {
            bool result = false;

            if (_context.ChangeTracker.HasChanges())
            {
                result = await _context.SaveChangesAsync() > 0;
            }

            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
