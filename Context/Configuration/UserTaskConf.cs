

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.ToTable("user_tasks");

        builder.HasKey(u => new { u.UserId, u.TaskId });

        builder.HasOne(userSchool => userSchool.User)
            .WithMany(user => user.UserTasks)
            .HasForeignKey(userSchool => userSchool.UserId);
    

        builder.HasOne(userSchool => userSchool.Task)
            .WithMany(school => school.UserTasks)
            .HasForeignKey(userSchool => userSchool.TaskId);       
    }
}