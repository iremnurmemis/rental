

using Core;
using FluentValidation;

namespace Business
{
    public class UserValidator:AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(c=>c.FirstName).NotEmpty();
            RuleFor(c=>c.LastName).NotEmpty();
            RuleFor(c=> c.Email).NotEmpty();

                
        }
    }
}
