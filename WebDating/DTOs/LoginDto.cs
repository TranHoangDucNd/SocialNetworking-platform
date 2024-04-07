using System.ComponentModel.DataAnnotations;

namespace WebDating.DTOs
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
