using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.Databases;
using blog.Infrastructure.Extensions;
//using blog.Infrastructure.Services;
using blog.Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace blog.Infrastructure.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        private readonly RepositoryDbContext _repositoryDbContext;
        //private readonly IPropertyMappingContainer _propertyMappingContainer;

        public PostRepository(
            RepositoryDbContext repositoryDbContext
            //IPropertyMappingContainer propertyMappingContainer
            ) 
            : base(repositoryDbContext)
        {
            _repositoryDbContext = repositoryDbContext;
            //_propertyMappingContainer = propertyMappingContainer;
        }

        //public async Task AddPostAsync(Post post)
        //{
        //    throw new NotImplementedException();
        //    //_repositoryDbContext.PostTags.AddAsync()
        //}
       

        public async Task<PaginatedList<Post>> GetAllPostAsync(PostParameters postParameters)
        {
            var query = _repositoryDbContext.Posts.AsQueryable();
            // 过滤
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
            if (!string.IsNullOrEmpty(postParameters.Orderby))
                query = query.OrderBy(postParameters.Orderby);

            // 翻页
            var count = await query.CountAsync();
            var data = await query
                .Skip(postParameters.PageSize * postParameters.PageIndex)
                .Take(postParameters.PageSize)
                .ToListAsync();

            return new PaginatedList<Post>(postParameters.PageSize, postParameters.PageIndex, count, data);
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
