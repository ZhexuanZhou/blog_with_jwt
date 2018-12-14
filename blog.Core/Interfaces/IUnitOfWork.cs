using System;
using System.Threading.Tasks;

namespace blog.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
