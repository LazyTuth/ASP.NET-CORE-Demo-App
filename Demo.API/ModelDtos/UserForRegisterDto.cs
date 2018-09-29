using System.ComponentModel.DataAnnotations;

namespace Demo.API.ModelDtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage="You must specify password between 4 and 8")]
        public string Password { get; set; }
    }
}