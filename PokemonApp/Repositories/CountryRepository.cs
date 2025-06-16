using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {

        public CountryRepository(AppDbContext db) : base(db)
        {

        }

        public async Task<Country> GetCountryByOwnerAsync(int ownerId)
        {
            return await _db.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersFromACountryAsync(int countryId)
        {
            return await _db.Owners.Where(o => o.CountryId == countryId).ToListAsync();
        }
    }
}
