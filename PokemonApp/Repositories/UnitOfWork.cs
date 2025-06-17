using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Interfaces;

namespace PokemonApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _db;

        public ICountryRepository Countries { get; }
        public ICategoryRepository Categories { get; }
        public IOwnerRepository Owners { get; }
        public IPokemonRepository Pokemons { get; }
        public IReviewerRepository Reviewers { get; }
        public IReviewRepository Reviews { get; }

        public UnitOfWork(AppDbContext db, ICountryRepository countries, ICategoryRepository categories, IOwnerRepository owners, IPokemonRepository pokemons, IReviewerRepository reviewers, IReviewRepository reviews)
        {
            _db = db;
            Countries = countries;
            Categories = categories;
            Owners = owners;
            Pokemons = pokemons;
            Reviewers = reviewers;
            Reviews = reviews;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }


        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
