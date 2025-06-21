using System.ComponentModel.DataAnnotations;

namespace PokemonApp.Models.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
