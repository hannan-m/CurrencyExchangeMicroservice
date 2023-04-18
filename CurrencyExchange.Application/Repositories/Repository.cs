using CurrencyExchange.Core.Interfaces;
using CurrencyExchange.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CurrencyExchange.Application.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly CurrencyExchangeDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(CurrencyExchangeDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            return entity != null;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
