using BLL.Authentication;
using BLL.Dtos.Subscription;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.FluentValidation
{
    public class SubscriptionAddDTOValidator : AbstractValidator<SubscriptionAddDTO>
    {
        public SubscriptionAddDTOValidator()
        {
            RuleFor(x => x.StartDate)
            .NotNull()
            .WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotNull()
                .WithMessage("End date is required.")
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be greater than start date.");

            RuleFor(x => x.NumSMSs)
                .NotEmpty();
        }
    }
}
