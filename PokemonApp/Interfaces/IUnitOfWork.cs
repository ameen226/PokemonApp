namespace PokemonApp.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        ICountryRepository Countries { get; }
        IOwnerRepository Owner { get; }
        IPokemonRepository Pokemons { get; }
        IReviewerRepository Reviewer { get; }
        IReviewRepository Reviews { get; }
        Task<int> SaveAsync();

    }
}
