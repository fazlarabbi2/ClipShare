using ClipShare.Core.IRepo;
using ClipShare.Entities;
using DataAccess.Data;

namespace ClipShare.DataAccess.Repo
{
    public class CategoryRepo(Context context) : BaseRepo<Category>(context), ICategoryRepo
    {
    }
}
