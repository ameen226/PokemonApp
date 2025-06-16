using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        ICollection<Review> GetReviewsOfAPokemon(int pokeId);
        bool DeleteReviews(List<Review> reviews);
    }
}
