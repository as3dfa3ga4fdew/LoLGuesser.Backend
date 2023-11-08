using Api.Models.Entities;
using System.Linq.Expressions;

namespace Api.Repositories.Interfaces
{
    public interface IAddressRepository : IRepository<AddressEntity>
    {
        public Task<IEnumerable<AddressEntity>> GetAllAsync(Expression<Func<AddressEntity, bool>> expression);
    }
}
