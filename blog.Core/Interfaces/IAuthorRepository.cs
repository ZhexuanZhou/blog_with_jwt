using System;
using System.Threading.Tasks;
using blog.Core.Entities;

namespace blog.Core.Interfaces
{
    public interface IAuthorRepository : IRepositoryBase<Author>
    {
        Task<Author> GetAuthorByIdAsync(int id);
    }
}
