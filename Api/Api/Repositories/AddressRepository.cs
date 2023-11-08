using Api.Contexts;
using Api.Models.Entities;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories
{
    public class AddressRepository : Repository<AddressEntity>, IAddressRepository
    {
        public AddressRepository(DataContext context, ILogger<IRepository<AddressEntity>> logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<AddressEntity>> GetAllAsync(Expression<Func<AddressEntity, bool>> expression)
        {
            try
            {
                return await _dbSet.Where(expression).ToListAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e, nameof(GetAllAsync));
                return new List<AddressEntity>();
            }
        }
    }
}
