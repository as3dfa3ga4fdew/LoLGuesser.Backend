using System.Linq.Expressions;

namespace Api.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);
        public Task<bool> UpdateAsync(TEntity entity);
        public Task<bool> CreateAsync(TEntity entity);
        public Task<bool> RemoveAsync(TEntity entity);
    }
}
