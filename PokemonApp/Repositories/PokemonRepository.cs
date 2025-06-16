using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Models.Dtos;

namespace PokemonApp.Repositories
{
    public class PokemonRepository : GenericRepository<Pokemon>, IPokemonRepository
    {
        public PokemonRepository(AppDbContext db) : base(db)
        {

        }

        public async Task CreatePokemonAsync(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _db.Owners.FirstOrDefault(o => o.Id == ownerId);
            var category = _db.Categories.FirstOrDefault(c => c.Id == ownerId);

            var pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon
            };


            var pokemonCategory = new PokemonCategory()
            {
                Pokemon = pokemon,
                Category = category
            };

            await _db.PokemonOwners.AddAsync(pokemonOwner);
            await _db.PokemonCategories.AddAsync(pokemonCategory);
            await _db.Pokemons.AddAsync(pokemon);

        }

        public async Task<Pokemon> GetPokemonAsync(string name)
        {
            return await _db.Pokemons.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<decimal> GetPokemonRatingAsync(int pokeId)
        {
            var reviews = _db.Reviews.Where(r => r.Pokemon.Id == pokeId);

            if (await reviews.CountAsync() <= 0)
                return 0;

            return ((decimal)await reviews.SumAsync(r => r.Rating) / await reviews.CountAsync());

        }

        public async Task<ICollection<Pokemon>> GetAllPokemonsAsync()
        {
            return await _db.Pokemons.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Pokemon> GetPokemonTrimToUpperAsync(PokemonDto pokemonCreate)
        {
            var pokemons = await GetAllPokemonsAsync();
            return  pokemons.Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
        }
    }
}
