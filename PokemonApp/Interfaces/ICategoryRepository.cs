using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<ICollection<Pokemon>> GetPokemonByCategoryAsync(int categoryId);
    }
}
