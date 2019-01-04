using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blog.Core.Entities;

namespace blog.Core.Interfaces
{
    public interface IPostTagRepository : IRepositoryBase<PostTag>
    {
        Task AddPostTagsAsync(Post post, List<Tag> tags);

    }
}
