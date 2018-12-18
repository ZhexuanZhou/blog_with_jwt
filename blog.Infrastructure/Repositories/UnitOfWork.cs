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
        private ICommentRepository _commentRepository;
        private IPostRepository _postRepository;
        private IPostTagRepository _postTagRepository;
        private ITagRepository _tagRepository;

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

        public ICommentRepository CommentRepository
        {
            get
            {
                if(_commentRepository == null)
                {
                    _commentRepository = new CommentRepository(_repositoryDbContext);
                }
                return _commentRepository;
            }
        }

        public IPostRepository PostRepository
        {
            get
            {
                if (_postRepository == null)
                {
                    _postRepository = new PostRepository(_repositoryDbContext);
                }
                return _postRepository;
            }
        }


        public IPostTagRepository PostTagRepository
        {
            get
            {
                if(_postTagRepository ==null)
                {
                    _postTagRepository = new PostTagRepository(_repositoryDbContext);
                }
                return _postTagRepository;
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                if(_tagRepository == null)
                {
                    _tagRepository = new TagRepository(_repositoryDbContext);
                }
                return _tagRepository;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _repositoryDbContext.SaveChangesAsync() > 0;
        }
    }
}
