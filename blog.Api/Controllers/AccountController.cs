using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using blog.Core.Entities;
using blog.Core.Interfaces;
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
        //private readonly RepositoryDbContext _repositoryDbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            var userIdentity = _mapper.Map<RegistrationViewModel, User>(model);
            if(await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest("User exist!");
            }
            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            //if (!result.Succeeded) 
                //return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _unitOfWork.AuthorRepository.AddAsync(new Author{ UserId = userIdentity.Id, Gender=model.Gender});
            await _unitOfWork.SaveChangesAsync(); 

            return new OkObjectResult("Account created");
        }
    }
}
