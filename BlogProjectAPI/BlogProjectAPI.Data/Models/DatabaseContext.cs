using Microsoft.EntityFrameworkCore;

namespace BlogProjectAPI.Data.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        { }
        public DbSet<TokenUser> TokenUsers { get; set; }
        public DbSet<AccessLogs> AccessLogs { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<TagsOfPosts> TagsOfPosts { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<ExceptionLogs> ExceptionLogs { get; set; }
    }
}
