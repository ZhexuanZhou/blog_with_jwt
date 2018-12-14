using System;
using System.Threading.Tasks;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.Databases;

namespace blog.Infrastructure.Repositories
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        private readonly RepositoryDbContext _repositoryDbContext;

        public AuthorRepository(RepositoryDbContext repositoryDbContext) : base(repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }
    }
}
