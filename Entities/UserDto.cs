using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

public class UserDto
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string UserName { get; set; }
    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string FirstName { get; set; }
    [Required]
    public string Password { get; set; }
    public string LastName { get; set; }
    [Required]
    [RegularExpression("^[0-9]{9}$")]
    public string PhoneNumber { get; set; }
    public IFormFile PhotoUrl { get; set; }

}