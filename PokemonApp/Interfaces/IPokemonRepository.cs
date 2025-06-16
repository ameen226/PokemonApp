using PokemonApp.Models;
using PokemonApp.Models.Dtos;

namespace PokemonApp.Interfaces
{
    public interface IPokemonRepository : IGenericRepository<Pokemon>
    {
        Task<ICollection<Pokemon>> GetAllPokemonsAsync();
        Task CreatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon);
        Task<Pokemon> GetPokemonAsync(string name);
        Task<Pokemon> GetPokemonTrimToUpperAsync(PokemonDto pokemonCreate);
        Task<decimal> GetPokemonRatingAsync(int pokeId);

    }
}
