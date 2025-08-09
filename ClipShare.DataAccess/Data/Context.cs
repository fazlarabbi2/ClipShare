using ClipShare.Core.Entities;
using ClipShare.DataAccess.Data.Config;
using ClipShare.Entities;
using DataAccess.Data.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public class Context(DbContextOptions<Context> options) : IdentityDbContext<AppUser, AppRole, int>(options)
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Video> Video { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new SubscribeConfig());
            builder.ApplyConfiguration(new LikeDislikeConfig());
            builder.ApplyConfiguration(new VideoViewConfig());
        }
    }
}
