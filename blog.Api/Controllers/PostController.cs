using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace blog.Api.Controllers
{
    [Route("api/post")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;


        public PostController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet(Name = "GetPosts")]
        public async Task<IActionResult> Get(PostParameters postParameters,
            [FromHeader(Name = "Accept")]string mediaType)
        {
            var postsList = await _unitOfWork.PostRepository
                .GetAllPostAsync(postParameters);
            if (postsList == null)
            {
                return NotFound();
            }
            var postResources = _mapper
                .Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postsList);
            return Ok(postResources);
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> Post(
            [FromBody] PostViewModel postAddViewModel)
        {
            var post = _mapper.Map<PostViewModel, Post>(postAddViewModel);
            var tags = postAddViewModel.Tags;
            if (tags.Count > 0)
                await _unitOfWork.PostTagRepository
                    .AddPostTagsAsync(post, tags);
            else
                await _unitOfWork.PostRepository.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();
            return Ok(post);
        }


    }
}
