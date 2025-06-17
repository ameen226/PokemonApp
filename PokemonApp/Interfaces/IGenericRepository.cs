using System.Linq.Expressions;

namespace PokemonApp.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {

        Task<bool> EntityExistsAsync(int id);
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);

    }
}
