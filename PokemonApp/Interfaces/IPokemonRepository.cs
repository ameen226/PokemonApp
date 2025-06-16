using PokemonApp.Models;
using PokemonApp.Models.Dtos;

namespace PokemonApp.Interfaces
{
    public interface IPokemonRepository : IGenericRepository<Pokemon>
    {
        Pokemon GetPokemon(string name);
        Pokemon GetPokemonTrimToUpper(PokemonDto pokemonCreate);
        decimal GetPokemonRating(int pokeId);

    }
}
