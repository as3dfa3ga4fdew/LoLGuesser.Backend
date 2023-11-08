using Api.Models.Entities;
using Api.Models.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces
{
    public interface IAddressService
    {
        public bool Validate(AddressSchema schema);
        public AddressEntity CreateEntity(AddressSchema schema, Guid userId);
        public Task<bool> CreateAsync(AddressEntity entity);
        public Task<IEnumerable<AddressEntity>> GetAllAsync(Guid userId);
        public Task<bool> RemoveAsync(Guid id, Guid userId);
        public Task<bool> UpdateAsync(AddressEntity entity);
    }
}
