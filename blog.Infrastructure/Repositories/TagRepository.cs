using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.Databases;

namespace blog.Infrastructure.Repositories
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        private readonly RepositoryDbContext _repositoryDbContext;

        public TagRepository(RepositoryDbContext repositoryDbContext) : base(repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        public async Task AddTagAsync(Tag tag)
        {
            if (!TagExist(tag))
            {
               await _repositoryDbContext.Tags.AddAsync(new Tag
               {
                   TagName = tag.TagName
               });
            }
        }

        public async Task AddTagRangeAsync(List<Tag> tags)
        {
            //throw new NotImplementedException();
            foreach(var t in tags)
            {
                if (!TagExist(t))
                {
                    await AddTagAsync(t);
                }
            }
        }

        public Tag GetTagByName(string name)
        {
            //throw new NotImplementedException();
            return _repositoryDbContext.Tags.FirstOrDefault(t => t.TagName.Equals(name));
        }

        private bool TagExist(Tag tag)
        {
            return _repositoryDbContext.Tags.Where(t => t.TagName.Equals(tag.TagName)).Any();
        }
    }
}
