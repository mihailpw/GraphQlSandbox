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
            builder.Entity<UserModel>().HasKey(x => x.Id);

            builder.Entity<UserRoleModel>().HasKey(x => x.Id);
            builder.Entity<UserRoleModel>().HasOne(x => x.User).WithMany(x => x.Roles);
            builder.Entity<UserRoleModel>().HasOne(x => x.Role);

            builder.Entity<RoleModel>().HasKey(x => x.Id);

            base.OnModelCreating(builder);
        }
    }
}