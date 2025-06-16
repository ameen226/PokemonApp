using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        Task<ICollection<Owner>> GetOwnerOfAPokemonAsync(int pokeId);
        Task<ICollection<Pokemon>> GetPokemonByOwnerAsync(int ownerId);
    }
}
