

using System.ComponentModel.DataAnnotations;

public class SchoolDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public IFormFile PhotoUrl { get; set; }
}