using BLL.Dtos.Campaign;
using Final.Project.BL;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.FluentValidation
{
    public class CampaignAddDTOValidator : AbstractValidator<CampaignAddDTO>
    {
        public CampaignAddDTOValidator() 
        {
            RuleFor(x => x.CampaignName)
                .NotEmpty();
            
            RuleFor(x => x.StartDate)
                .NotEmpty();
            
            RuleFor(x => x.ContentMessage)
                .NotEmpty()
                .MaximumLength(200).WithMessage("The Content Message should be 160 characters");
           
            RuleFor(x => x.SenderName)
                .NotEmpty();
            
            RuleFor(x => x.Status)
                .NotEmpty();
            
            RuleForEach(x => x.PhoneNumber)
                .NotEmpty().MaximumLength(12)
                .WithMessage("The Phone Number Must be 12 digits");
        }
    }
}
