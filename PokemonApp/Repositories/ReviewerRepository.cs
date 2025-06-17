using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
    public class ReviewerRepository : GenericRepository<Reviewer>, IReviewerRepository
    {
        public ReviewerRepository(AppDbContext db) : base(db)
        {

        }

        public override async Task<Reviewer> GetByIdAsync(int reviewerId)
        {
            return await _db.Reviewers.Where(r => r.Id == reviewerId).Include(r => r.Reviews).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Review>> GetReviewsByReviewerAsync(int reviewerId)
        {
            return await _db.Reviews.Where(r => r.ReviewerId == reviewerId).ToListAsync();
        }
    }
}
