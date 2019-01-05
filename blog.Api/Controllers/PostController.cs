using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using blog.Core.Entities;
using blog.Core.Interfaces;
//using blog.Infrastructure.Services;
using blog.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace blog.Api.Controllers
{
    [Route("api/post")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IUrlHelper _urlHelper;

        public PostController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<User> userManager,
            IUrlHelper urlHelper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetPosts")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(PostParameters postParameters,
            [FromHeader(Name = "Accept")]string mediaType)
        {
            var postsList = await _unitOfWork.PostRepository
                .GetAllPostAsync(postParameters);

            var postResources = _mapper
                .Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postsList);

            //翻页连接
            var previousPageLink = postsList.HasPrevious ?
                    CreatePostUri(postParameters, PaginationResourceUriType.PreviousPage) : null;

            var nextPageLink = postsList.HasNext ?
               CreatePostUri(postParameters, PaginationResourceUriType.NextPage) : null;

            // 翻页元数据
            var meta = new
            {
                PageSize = postsList.PageSize,
                PageIndex = postsList.PageIndex,
                TotalItemCount = postsList.TotalItemCount,
                PageCount = postsList.PageCount,
                previousPageLink,
                nextPageLink
            };
            Response.Headers.Add("X-Pagenation", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            return Ok(postResources);
        }

        private string CreatePostUri(PostParameters postParameters, PaginationResourceUriType uriType)
        {
            // 针对 [HttpGet(Name = "GetPosts")] 生成新的 PostParameters
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameter = new
                    {
                        pageIndex = postParameters.PageIndex - 1,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.Orderby,
                        fields = postParameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", previousParameter);
                case PaginationResourceUriType.NextPage:
                    var nextParameter = new
                    {
                        pageIndex = postParameters.PageIndex + 1,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.Orderby,
                        fields = postParameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", nextParameter);
                default:
                    var currentParameter = new
                    {
                        pageIndex = postParameters.PageIndex,
                        pageSize = postParameters.PageSize,
                        orderBy = postParameters.Orderby,
                        fields = postParameters.Fields
                    };
                    return _urlHelper.Link("GetPosts", currentParameter);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id, string fields = null)
        {
            var post = await _unitOfWork.PostRepository.GetPostByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var postResource = _mapper.Map<Post, PostViewModel>(post);
            return Ok(postResource);

        }

        [HttpPost(Name = "CreatePost")]
        [Authorize]
        public async Task<IActionResult> Post(
            [FromBody] PostViewModel postAddViewModel)
        {
            var post = _mapper.Map<PostViewModel, Post>(postAddViewModel);
            var tagVMs = postAddViewModel.Tags;
            var tags = _mapper.Map<List<TagViewModel>, List<Tag>>(tagVMs);

            if (tags.Count > 0)
            {
                await _unitOfWork.PostTagRepository
                    .AddPostTagsAsync(post, tags);
            }
            else
            {
                await _unitOfWork.PostRepository.AddAsync(post);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(post);
        }

        [HttpPatch("{id}", Name = "PartiallyUpdatePost")]
        [Authorize]
        public async Task<IActionResult> PartiallyUpdateCityForCountry(int id,
            [FromBody] JsonPatchDocument<PostUpdateViewModel> patchDoc)
        {
            var post = await _unitOfWork.PostRepository.GetPostByIdAsync(id);
            var postToPatch = _mapper.Map<PostUpdateViewModel>(post);
            patchDoc.ApplyTo(postToPatch, ModelState);
            _mapper.Map(postToPatch, post);
            post.LastModify = DateTime.Now;
            _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
            return Ok(post.Title);
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        [Authorize]
        public async Task<IActionResult> DeletPost(int id)
        {
            var post = await _unitOfWork.PostRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _unitOfWork.PostRepository.Delete(post);

            if (!await _unitOfWork.SaveChangesAsync())
            {
                throw new Exception($"Delete post {id} when saving");
            }
            return NoContent();
        }

    }
}
