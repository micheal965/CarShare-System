using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Youth_Innovation_System.Core.Entities;

namespace Youth_Innovation_System.Repository.Data.Config
{
    public class PostConfiguration : IEntityTypeConfiguration<CarPost>
    {
        public void Configure(EntityTypeBuilder<CarPost> builder)
        {
            //builder.HasMany(p => p.Reacts)
            //    .WithOne(r => r.post)
            //    .HasForeignKey(r => r.postId)
            //    .OnDelete(DeleteBehavior.Cascade);//Delete reacts if post is deleted

            //builder.HasMany(p => p.Comments)
            //    .WithOne(r => r.post)
            //    .HasForeignKey(r => r.postId)
            //    .OnDelete(DeleteBehavior.Cascade);//Delete comments if post is deleted

            builder.HasMany(p => p.postImages)
                .WithOne(pi => pi.post)
                .HasForeignKey(pi => pi.PostId)
                .OnDelete(DeleteBehavior.Cascade);//Delete postimages if post deleted

        }
    }
}
