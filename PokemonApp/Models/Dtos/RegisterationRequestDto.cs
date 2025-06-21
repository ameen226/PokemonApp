using System.ComponentModel.DataAnnotations;

namespace PokemonApp.Models.Dtos
{
    public class RegisterationRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
