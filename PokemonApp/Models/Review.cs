namespace PokemonApp.Models
{
    public class Review : BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }

        public int ReviewerId { get; set; }
        public Reviewer Reviewer { get; set; }

        public int PokemonId { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
