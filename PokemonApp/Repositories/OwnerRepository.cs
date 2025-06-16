using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(AppDbContext db) : base(db)
        {

        }

        public async Task<ICollection<Owner>> GetOwnerOfAPokemonAsync(int pokeId)
        {
            return await _db.PokemonOwners.Where(po => po.PokemonId == pokeId).Select(po => po.Owner).ToListAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonByOwnerAsync(int ownerId)
        {
            return await _db.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToListAsync();
        }
    }
}
