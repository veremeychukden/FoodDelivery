using System.ComponentModel.DataAnnotations;

namespace DTO.Requests
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}