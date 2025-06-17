using PokemonApp.Models;
namespace PokemonApp.Interfaces
{
    public interface IReviewerRepository : IGenericRepository<Reviewer>
    {

        Task<ICollection<Review>> GetReviewsByReviewerAsync(int reviewerId);
    }
}
