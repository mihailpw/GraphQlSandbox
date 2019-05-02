using GQL.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GQL.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>(b =>
            {
                b.HasKey(x => x.Id);
            });

            builder.Entity<UserRoleModel>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.User).WithMany(x => x.Roles);
                b.HasOne(x => x.Role);
            });

            builder.Entity<RoleModel>(b =>
            {
                b.HasKey(x => x.Id);
            });

            builder.Entity<UserFriendModel>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.User).WithMany(x => x.Friends);
                b.HasOne(x => x.Friend);
            });

            base.OnModelCreating(builder);
        }
    }
}