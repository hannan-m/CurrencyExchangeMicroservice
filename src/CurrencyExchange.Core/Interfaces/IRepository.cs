using System.Linq.Expressions;

namespace CurrencyExchange.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAll();
        Task<TEntity> GetAsync(int id);
        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
