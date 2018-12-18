using System;
using System.Threading.Tasks;
using blog.Core.Entities;
using blog.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace blog.Api.Controllers
{
    [Route("api/post")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetPosts")]
        public async Task<IActionResult> Get(PostParameters postParameters,
            [FromHeader(Name = "Accept")]string mediaType)
        {
            var postsList = await _unitOfWork.PostRepository.GetAllPostAsync(postParameters);
            return Ok(postsList);
        }
    }
}
