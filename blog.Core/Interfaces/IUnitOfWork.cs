using System;
using System.Threading.Tasks;

namespace blog.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangeAsync();
    }
}
