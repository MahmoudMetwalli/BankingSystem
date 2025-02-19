namespace BankingSystemAPIs.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(Guid id,T entity);
        Task DeleteAsync(Guid id);
    }

}
