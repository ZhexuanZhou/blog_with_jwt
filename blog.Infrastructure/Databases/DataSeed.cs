using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.Core.Entities;
using blog.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace blog.Infrastructure.Databases
{
    public class DataSeed
    {
        public static async Task SeedAsync(
            RepositoryDbContext repositoryDbContext, 
            ILoggerFactory loggerFactory, 
            //UserManager<User> userManager,
            int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                var posts = new List<Post>
                {
                    new Post
                    {
                        //Id = 1,
                        Title = "This is artical 1",
                        Body = "This is body of artical 1",
                        CreateTime = DateTime.Now,
                        LastModify = DateTime.Now,
                        AuthorId = 1

                    },
                    new Post
                    {
                        //Id = 2,
                        Title = "This is artical 2",
                        Body = "This is body of artical 2",
                        CreateTime = DateTime.Now,
                        LastModify = DateTime.Now,
                        AuthorId = 2

                    },
                    new Post
                    {
                        //Id = 3,
                        Title = "This is artical 3",
                        Body = "This is body of artical 3",
                        CreateTime = DateTime.Now,
                        LastModify = DateTime.Now,
                        AuthorId = 3

                    }
                };

                var tags = new List<Tag>
                {
                    new Tag
                    {
                        //Id=1,
                        TagName = "Sport"

                    },
                    new Tag
                    {
                        //Id=2,
                        TagName = "Markup"

                    },
                    new Tag
                    {
                        //Id=3,
                        TagName = "Food"

                    },
                    new Tag
                    {
                        //Id = 4,
                        TagName = "Movie"

                    }
                };

                var coms = new List<Comment>
                {
                    new Comment
                    {
                        //Id = 1,
                        ParentId = 0,
                        Email = "guest1@example.com",
                        Body = "This is comment 1 for Artical 1",
                        CreateDate = DateTime.Now,
                        Post = posts[0],
                        AuthorId = 1
                    },
                    new Comment
                    {
                        //Id = 2,
                        ParentId = 0,
                        Email = "guest2@example.com",
                        Body = "This is comment 2 for Artical 2",
                        CreateDate = DateTime.Now,
                        Post = posts[1],
                        AuthorId = 2
                    },
                    new Comment
                    {
                        //Id = 3,
                        ParentId = 0,
                        Email = "guest3@example.com",
                        Body = "This is comment 3 for Artical 3",
                        CreateDate = DateTime.Now,
                        Post = posts[2],
                        AuthorId = 3
                    },
                    new Comment
                    {
                        //Id = 4,
                        ParentId = 1,
                        Email = "Peng@zhexuan.com",
                        Body = "This is comment 1.1 for Artical 1",
                        CreateDate = DateTime.Now,
                        Post = posts[0],
                        AuthorId = 3
                    }
                };

                if (!repositoryDbContext.PostTags.Any())
                {
                    repositoryDbContext.PostTags.AddRange(
                        new List<PostTag>
                        {
                            new PostTag { Post = posts[0], Tag = tags[0] },
                            new PostTag { Post = posts[0], Tag = tags[1] },
                            new PostTag { Post = posts[1], Tag = tags[2] },
                            new PostTag { Post = posts[1], Tag = tags[3] },
                            new PostTag { Post = posts[2], Tag = tags[0] },
                            new PostTag { Post = posts[2], Tag = tags[1] },
                            new PostTag { Post = posts[2], Tag = tags[2] },
                            new PostTag { Post = posts[2], Tag = tags[3] }

                        }
                    );
                }

                if (!repositoryDbContext.Comments.Any())
                {
                    repositoryDbContext.Comments.AddRange(coms);
                }

                await repositoryDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<DataSeed>();
                    logger.LogError(ex.Message);
                    
                    await SeedAsync(repositoryDbContext, loggerFactory, retryForAvailability);
                }
            }
        }
    }
}
