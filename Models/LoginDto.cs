using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;

namespace ATMBank.Models{
    public class LoginDto{
        [Key]//Annotation -> primary key
        public string Email { get; set; }
        public string Password { get; set; }


        public static implicit operator LoginDto(ClaimsPrincipal v)
        {
            throw new NotImplementedException();
        }

    }
}