using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
    }
}
