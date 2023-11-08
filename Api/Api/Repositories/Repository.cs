using Api.Contexts;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Api.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public readonly DataContext _context;
        public readonly DbSet<TEntity> _dbSet;
        public readonly ILogger<IRepository<TEntity>> _logger;
        public Repository(DataContext context, ILogger<IRepository<TEntity>> logger)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _logger = logger;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> RemoveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
