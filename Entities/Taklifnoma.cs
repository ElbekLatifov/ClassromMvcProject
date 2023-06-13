

using System.ComponentModel.DataAnnotations;

public class Taklifnoma
{
    [Key]
    public Guid Id { get; set; }

    public Guid ToUserId { get; set; }

    public Guid FromUserId { get; set; }
    public User FromUser { get; set; }

    public Guid FanId { get; set; }
    public Fan Fan { get; set; }

    public bool IsJoined { get; set; }
}