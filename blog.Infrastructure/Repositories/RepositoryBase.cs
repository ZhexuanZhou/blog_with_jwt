using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace blog.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class 
    {
        private readonly RepositoryDbContext _repositoryDbContext;

        public RepositoryBase(RepositoryDbContext repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        public void Add(TEntity entity)
        {
            _repositoryDbContext.Set<TEntity>().Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _repositoryDbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _repositoryDbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<ICollection<TEntity>> FindAllAsnyc()
        {
            return await _repositoryDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<ICollection<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return await _repositoryDbContext.Set<TEntity>().Where(expression).ToListAsync();
        }

        public void Update(TEntity entity)
        {
            _repositoryDbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
