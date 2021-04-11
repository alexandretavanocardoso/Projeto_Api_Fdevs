using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IDbContext _dbContext;

        public BaseRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _dbContext.Connection.GetAsync<T>(id);

            if (item == null)
                return false;

            await _dbContext.Connection.DeleteAsync(item);
            return true;
        }

        public async Task<T> InsertAsync(T item)
        {
            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();

            item.CreateAt = DateTime.Now;

            await _dbContext.Connection.InsertAsync(item);

            return item;
        }

        public async Task<T> SelectAsync(Guid id)
        {
            return await _dbContext.Connection.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            return await _dbContext.Connection.GetAllAsync<T>();
        }

        public async Task<T> UpdateAsync(T item)
        {
            item.UpdateAt = DateTime.Now;

            await _dbContext.Connection.UpdateAsync(item);

            return item;
        }
    }
}
