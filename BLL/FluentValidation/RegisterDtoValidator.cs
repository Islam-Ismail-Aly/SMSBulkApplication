using BLL.Authentication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.FluentValidation
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("FirstName is required.");
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("LastName is required.");
            
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("UserName is required."); 
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(130)
                .WithMessage("Password is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(256)
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email address.");
        }
    }
}
