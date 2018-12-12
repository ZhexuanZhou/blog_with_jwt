using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using blog.Core.Entities;
using blog.Infrastructure.Databases;
using blog.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace blog.Api.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly RepositoryDbContext _repositoryDbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountController(
            IMapper mapper, 
            RepositoryDbContext repositoryDbContext,
            UserManager<User> userManager)
        {
            _repositoryDbContext = repositoryDbContext;
            _mapper = mapper;
            _userManager = userManager;

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            var userIdentity = _mapper.Map<RegistrationViewModel, User>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            //if (!result.Succeeded) 
                //return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _repositoryDbContext.Authors.AddAsync(new Author{ UserId = userIdentity.Id});
            await _repositoryDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}
