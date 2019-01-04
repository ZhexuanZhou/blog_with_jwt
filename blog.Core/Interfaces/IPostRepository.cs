using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blog.Core.Entities;

namespace blog.Core.Interfaces
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        Task<PaginatedList<Post>> GetAllPostAsync(PostParameters postParameters);
        Task<Post> GetPostByIdAsync(int id);

        //Task AddPostAsync(Post post);
    }
}
