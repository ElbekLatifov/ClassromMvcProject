

using System.ComponentModel.DataAnnotations;

public class School
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
    public List<UserSchool> Users {get; set;}
    public List<Fan> Fans { get; set; }

}