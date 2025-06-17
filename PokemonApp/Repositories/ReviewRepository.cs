using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {

        public ReviewRepository(AppDbContext db) : base(db)
        {

        }


        public void DeleteReviews(List<Review> reviews)
        {
            _db.Reviews.RemoveRange(reviews);
        }

        public async Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokeId)
        {
            return await _db.Reviews.Where(r => r.PokemonId == pokeId).ToListAsync();
        }
    }
}
