using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;
namespace ATMBank.Models{
public class User
{
    [Key]
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // In a real-world app, this should be hashed!
}

}