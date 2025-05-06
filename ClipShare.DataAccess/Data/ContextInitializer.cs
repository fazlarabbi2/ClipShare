using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace ClipShare.DataAccess.Data
{
    public static class ContextInitializer
    {
        public static void Initialize(Context context)
        {
            if (context.Database.GetAppliedMigrations().Count() > 0)
            {
                context.Database.Migrate();
            }
        }
    }
}
