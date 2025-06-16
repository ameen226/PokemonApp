namespace PokemonApp.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {

        bool EntityExists(int id);
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetById(int id);
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);

    }
}
