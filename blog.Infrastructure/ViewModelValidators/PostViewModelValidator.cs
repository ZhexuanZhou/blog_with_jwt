using System;
using blog.Infrastructure.ViewModels;
using FluentValidation;

namespace blog.Infrastructure.ViewModelValidators
{
    public class PostViewModelValidator : AbstractValidator<PostViewModel>
    {
        public PostViewModelValidator()
        {
            RuleFor(x => x.AuthorId).NotNull().WithMessage("AuthorId should not be none!");
        }
    }
}
