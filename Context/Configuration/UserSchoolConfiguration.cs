

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserSchoolConfiguration : IEntityTypeConfiguration<UserSchool>
{
    public void Configure(EntityTypeBuilder<UserSchool> builder)
    {
        builder.ToTable("user_school");

        builder.HasKey(u => new { u.UserId, u.SchoolId });

        builder.HasOne(userSchool => userSchool.User)
            .WithMany(user => user.Schools)
            .HasForeignKey(userSchool => userSchool.UserId);
    

        builder.HasOne(userSchool => userSchool.School)
            .WithMany(school => school.Users)
            .HasForeignKey(userSchool => userSchool.SchoolId);
            
    }
}