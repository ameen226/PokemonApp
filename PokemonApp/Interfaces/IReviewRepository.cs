using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<ICollection<Review>> GetReviewsOfAPokemonAsync(int pokeId);
        void DeleteReviews(List<Review> reviews);
    }
}
