namespace PokemonApp.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        ICountryRepository Countries { get; }
        IOwnerRepository Owners { get; }
        IPokemonRepository Pokemons { get; }
        IReviewerRepository Reviewers { get; }
        IReviewRepository Reviews { get; }
        Task<int> SaveAsync();

    }
}
