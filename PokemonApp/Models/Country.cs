namespace PokemonApp.Models
{
    public class Country : BaseModel
    {
        public string Name { get; set; }
        public ICollection<Owner> Owners { get; set; }

    }
}
