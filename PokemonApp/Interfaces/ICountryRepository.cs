using PokemonApp.Models;
namespace PokemonApp.Interfaces
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country> GetCountryByOwnerAsync(int ownerId);
        Task<ICollection<Owner>> GetOwnersFromACountryAsync(int countryId);

    }
}
