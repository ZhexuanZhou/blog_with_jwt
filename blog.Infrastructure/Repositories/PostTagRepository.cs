using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;

namespace blog.Infrastructure.Repositories
{
    public class PostTagRepository 
        : RepositoryBase<PostTag>, IPostTagRepository
    {
        private readonly RepositoryDbContext _repositoryDbContext;

        public PostTagRepository(RepositoryDbContext repositoryDbContext) 
            : base(repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        public async Task AddPostTagsAsync(Post post, List<Tag> tags)
        {
            foreach (var tag in tags)
            {
                Tag targetedTag = _repositoryDbContext.Tags
                    .Where(t => t.TagName.Equals(tag.TagName))
                    .FirstOrDefault();

                if (targetedTag == null)
                {
                    targetedTag = new Tag
                    {
                        TagName = tag.TagName
                    };
                    _repositoryDbContext.Tags.Add(targetedTag);
                }

                await _repositoryDbContext.PostTags.AddAsync(new PostTag
                {
                    Post = post,
                    TagId = targetedTag.Id
                });
            }
        }
    }
}
