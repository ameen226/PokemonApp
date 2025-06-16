using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<ICollection<Pokemon>> GetPokemonByCategoryAsync(int categoryId)
        {
            return await _db.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(c => c.Pokemon).ToListAsync();
        }
    }
}
