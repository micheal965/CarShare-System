using Microsoft.EntityFrameworkCore;
using Youth_Innovation_System.Core.Entities;
using Youth_Innovation_System.Core.Entities.Chat;

namespace Youth_Innovation_System.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CarPost> carPosts { get; set; }
        public DbSet<CarFeedback> CarFeedbacks { get; set; }

        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<RentalApplication> RentalApplications { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
