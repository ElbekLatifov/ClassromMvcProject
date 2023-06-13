
using System.ComponentModel.DataAnnotations;

public class TaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public string Status { get; set; }
    public DateTime EndDate { get; set; }
    public ushort MaxBall { get; set; }
}