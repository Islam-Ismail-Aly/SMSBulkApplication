using BLL.Authentication;
using Final.Project.BL;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.FluentValidation
{
    public class ChangePasswordValidator : AbstractValidator<UserChangepassDto>
    {
        public ChangePasswordValidator() 
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("OldPassword is required.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("NewPassword is required.");
            RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage("ConfirmNewPassword is required.");
        }
    }
}
