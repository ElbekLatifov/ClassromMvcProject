using System.ComponentModel.DataAnnotations;

public class TaskEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime StartDate { get; set; }
    public string Status { get; set; }
    public DateTime EndDate { get; set; }
    public Guid FanId { get; set; }
    public Fan Fan { get; set;}
    public ushort MaxBall { get; set; }
    public List<UserTask> UserTasks  { get; set; }
    public List<TaskComment> Comments { get; set; }
}