using PokemonApp.Models;
namespace PokemonApp.Interfaces
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersFromACountry(int countryId);

    }
}
