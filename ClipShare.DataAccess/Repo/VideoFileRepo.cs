using ClipShare.Core.Entities;
using ClipShare.Core.IRepo;
using DataAccess.Data;

namespace ClipShare.DataAccess.Repo
{
    public class VideoFileRepo(Context context) : BaseRepo<VideoFile>(context), IVideoFileRepo
    {

    }
}
