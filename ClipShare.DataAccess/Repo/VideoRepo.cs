using ClipShare.Core.IRepo;
using ClipShare.Entities;
using DataAccess.Data;

namespace ClipShare.DataAccess.Repo
{
    public class VideoRepo(Context context) : BaseRepo<Video>(context), IVideoRepo
    {
    }
}
