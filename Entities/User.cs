

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public string PhotoUrl {get; set;}
    public List<UserSchool> Schools {get; set;}
    public List<UserFan> UserFans { get; set; }
    public List<Taklifnoma> Requests { get; set; }
    public List<UserTask> UserTasks { get; set; }
    public List<TaskComment> TaskComments { get; set; }
}