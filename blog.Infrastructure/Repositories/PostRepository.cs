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
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        private readonly RepositoryDbContext _repositoryDbContext;

        public PostRepository(RepositoryDbContext repositoryDbContext) : base(repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
        }

        //public async Task AddPostAsync(Post post)
        //{
        //    throw new NotImplementedException();
        //    //_repositoryDbContext.PostTags.AddAsync()
        //}
       

        public async Task<PaginatedList<Post>> GetAllPostAsync(PostParameters postParameters)
        {
            var query = _repositoryDbContext.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(postParameters.Title))
            {
                // 在PostParameters中添加title，可根据需求更改
                var title = postParameters.Title.ToLowerInvariant();
                // query = query.Where(x=>x.Title.ToLowerInvariant().Contains(title));
                query = query.Where(x => x.Title.ToLowerInvariant() == title);

            }
            query = query.Include(p => p.PostTags).ThenInclude(pt=>pt.Tag)
                .Include(p=>p.Author).ThenInclude(a=>a.User);
            //排序
            //query = query.ApplySort(postParameters.Orderby, _propertyMappingContainer.Resolve<PostResource, Post>());

            // 翻页query = query.OrderBy(x=>x.Id);
            var count = await query.CountAsync();

            var data = await query
                .Skip(postParameters.PageSize * postParameters.PageIndex)
                .Take(postParameters.PageSize)
                .ToListAsync();

            return new PaginatedList<Post>(postParameters.PageIndex, postParameters.PageSize, count, data);
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            var query = _repositoryDbContext.Posts.AsQueryable();
            query = query.Where(p => p.Id.Equals(id))
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag);
                //.Include(p => p.Author)
                //.ThenInclude(a => a.User);
            return await query.FirstOrDefaultAsync();
        }
    }
}
