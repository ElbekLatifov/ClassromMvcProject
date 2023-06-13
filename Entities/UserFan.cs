

public class UserFan
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid FanId { get; set; }
    public Fan Fan { get; set; }
    public EUserFan Type { get; set; }
}