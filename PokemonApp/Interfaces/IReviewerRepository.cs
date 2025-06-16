using PokemonApp.Models;
namespace PokemonApp.Interfaces
{
    public interface IReviewerRepository : IGenericRepository<Reviewer>
    {
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
    }
}
