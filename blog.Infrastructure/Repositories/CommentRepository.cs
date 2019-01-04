using System;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.Databases;

namespace blog.Infrastructure.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        private readonly RepositoryDbContext _repositoryDbContext;

        public CommentRepository(RepositoryDbContext repositoryDbContext) : base(repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }
    }
}
