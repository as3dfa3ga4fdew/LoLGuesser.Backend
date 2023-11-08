using Api.Exceptions;
using Api.Models.Dtos;
using Api.Models.Entities;
using Api.Models.Schemas;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Api.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;

        public AddressService(IAddressRepository repository)
        {
            _repository = repository;
        }

        public bool Validate(AddressSchema schema)
        {
            if (schema == null)
                return false;

            if (string.IsNullOrEmpty(schema.FirstName) || schema.FirstName.Length < 3 || schema.FirstName.Length > 50)
                return false;

            if (string.IsNullOrEmpty(schema.LastName) || schema.LastName.Length < 3 || schema.LastName.Length > 50)
                return false;

            if (string.IsNullOrEmpty(schema.Street) || schema.Street.Length < 3 || schema.Street.Length > 100)
                return false;

            if (string.IsNullOrEmpty(schema.PostalCode) || schema.PostalCode.Length < 5 || schema.PostalCode.Length > 10)
                return false;

            if (string.IsNullOrEmpty(schema.City) || schema.City.Length < 2 || schema.City.Length > 50)
                return false;

            return true;
        }
        public AddressEntity CreateEntity(AddressSchema schema, Guid userId)
        {
            return new AddressEntity()
            {
                Title = schema.Title,
                FirstName = schema.FirstName,
                LastName = schema.LastName,
                Street = schema.Street,
                PostalCode = schema.PostalCode,
                City = schema.City,
                UserId = userId
            };
        }

        public async Task<bool> CreateAsync(AddressEntity entity)
        {
            if(entity == null)
                throw new ArgumentNullException(nameof(entity));

            return await _repository.CreateAsync(entity);
        }

        public async Task<IEnumerable<AddressEntity>> GetAllAsync(Guid userId)
        {
            return await _repository.GetAllAsync(x => x.UserId == userId);
        }

        public async Task<bool> RemoveAsync(Guid id, Guid userId)
        {
            AddressEntity entity = await _repository.GetAsync(x => x.Id == id && x.UserId == userId);
            if (entity == null)
                throw new AddressNotFoundException(nameof(RemoveAsync) + " AddressId: " + id + " UserId: " + userId);

            return await _repository.RemoveAsync(entity);
        }

        public async Task<bool> UpdateAsync(AddressEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            AddressEntity foundEntity = await _repository.GetAsync(x => x.Id == entity.Id && x.UserId == entity.UserId);
            if(foundEntity == null)
                throw new AddressNotFoundException(nameof(RemoveAsync) + " AddressId: " + entity.Id + " UserId: " + entity.UserId);

            return await _repository.UpdateAsync(entity);
        }
    }
}
