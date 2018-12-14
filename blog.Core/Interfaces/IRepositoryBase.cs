using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using blog.Core.Entities;

namespace blog.Core.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        // Find
        Task<ICollection<TEntity>> FindAllAsnyc();
        Task<ICollection<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression);

        // Add
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);

        // Update
        void Update(TEntity entity);

        // Delete
        void Delete(TEntity entityr);
    }
}
