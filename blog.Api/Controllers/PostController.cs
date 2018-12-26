using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using blog.Core.Entities;
using blog.Core.Interfaces;
using blog.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace blog.Api.Controllers
{
    [Route("api/post")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetPosts")]
        public async Task<IActionResult> Get(PostParameters postParameters,
            [FromHeader(Name = "Accept")]string mediaType)
        {
            var postsList = await _unitOfWork.PostRepository.GetAllPostAsync(postParameters);
            var postResources = _mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postsList);
            return Ok(postResources);
        }
    }
}
