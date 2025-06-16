using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
    }
}
