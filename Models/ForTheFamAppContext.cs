using Microsoft.EntityFrameworkCore;

namespace ForTheFamApp.Models
{
    public class ForTheFamAppContext : DbContext
    {
        public ForTheFamAppContext(DbContextOptions options) : base(options) { }

        // for every model / entity that is going to be part of the db
        // the names of these properties will be the names of the tables in the db
        //public DbSet<User> Users { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<ForumPost> ForumPosts { get; set; }

        public DbSet<ForumComment> ForumComments { get; set; }

        public DbSet<ProfilePost> ProfilePosts { get; set; }

        public DbSet<ProductPost> ProductPosts { get; set; }

    }
}