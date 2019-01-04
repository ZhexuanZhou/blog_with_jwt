using System;
using System.Threading.Tasks;

namespace blog.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        ICommentRepository CommentRepository { get; }
        IPostRepository PostRepository { get; }
        IPostTagRepository PostTagRepository { get; }
        ITagRepository TagRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
