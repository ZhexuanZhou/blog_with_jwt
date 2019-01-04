using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blog.Core.Entities;

namespace blog.Core.Interfaces
{
    public interface ITagRepository : IRepositoryBase<Tag>
    {
        Task AddTagAsync(Tag tag);
        Task AddTagRangeAsync(List<Tag> tags);

        Tag GetTagByName(string name);
    }
}
