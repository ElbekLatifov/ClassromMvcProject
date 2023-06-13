using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Data.DbContext;
using Microsoft.AspNetCore.Identity;

namespace ClassRoom.Data.DbContext;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<School> SchoolSet { get; set; }
    public DbSet<Fan> Fans { get; set; }
    public DbSet<UserFan> UsersFans { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    public DbSet<Taklifnoma> Requests { get; set; }
    public DbSet<UserSchool> UserSchools { get; set;}
    public DbSet<TaskEntity> Tasks { get; set;}
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<TaskComment> Comments { get; set; }
     //public DbSet<School> Schools { get;set;}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //new UserSchoolConfiguration().Configure(builder.Entity<UserSchool>());
        // builder.ApplyConfiguration(new UserSchoolConfiguration());
        // builder.ApplyConfiguration(new UserFanConfiguration());
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<User>().ToTable("users");

        builder.Entity<User>().Property(p => p.FirstName)
            .HasColumnName("first_name")
                .HasMaxLength(50)
                    .IsRequired();

        builder.Entity<User>().Property(p => p.LastName)
            .HasColumnName("last_name")
                .HasMaxLength(50)
                    .IsRequired(false);

        builder.Entity<User>().Property(p => p.PhotoUrl)
            .HasColumnName("photo_url");

        builder.Entity<Fan>().Property(p=>p.Discription)
            .HasColumnName("discription_fan")
                .HasMaxLength(1150).IsRequired(false);
    
        builder.Entity<Fan>().Property(p=>p.Name)
            .HasColumnName("fan_name")
                .HasMaxLength(100).IsRequired();
        //builder.Entity<UserSchool>().HasNoKey();
        
    }
}