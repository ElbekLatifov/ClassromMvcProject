

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserFanConfiguration : IEntityTypeConfiguration<UserFan>
{
    public void Configure(EntityTypeBuilder<UserFan> builder)
    {
        builder.ToTable("user_fan");

        builder.HasKey(u => new { u.UserId, u.FanId });

        builder.HasOne(userSchool => userSchool.User)
            .WithMany(user => user.UserFans)
            .HasForeignKey(userSchool => userSchool.UserId);
    

        builder.HasOne(userSchool => userSchool.Fan)
            .WithMany(school => school.FanUsers)
            .HasForeignKey(userSchool => userSchool.FanId);       
    }
}