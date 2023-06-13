

using System.ComponentModel.DataAnnotations;

public class Fan
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Discription { get; set; }
    public List<UserFan> FanUsers { get; set; }
    public Guid SchoolId { get; set; }
    public School School { get; set; }
    public List<TaskEntity> Tasks { get; set; }
}