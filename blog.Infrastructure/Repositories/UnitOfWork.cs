using System;
using System.Threading.Tasks;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.Databases;

namespace blog.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryDbContext _repositoryDbContext;
        private IAuthorRepository _authorRepository;

        public UnitOfWork(RepositoryDbContext repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                if(_authorRepository == null)
                {
                    _authorRepository = new AuthorRepository(_repositoryDbContext);
                }
                return _authorRepository;
            }
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _repositoryDbContext.SaveChangesAsync() > 0;
        }
    }
}
