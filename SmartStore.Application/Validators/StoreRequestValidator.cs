using FluentValidation;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using SmartStore.Domain.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStore.Application.Validators
{
    public class StoreRequestValidator:AbstractValidator<StoreRequestDto>
    {
        private readonly IMessageService _messageService;

        public StoreRequestValidator(IMessageService messageService)
        {
            _messageService = messageService;

            RuleFor(x => x.CommercialRegistrationNumber)
              .Matches(@"^\d{10}$") 
              .When(x => !string.IsNullOrEmpty(x.CommercialRegistrationNumber))
              .WithMessage(messageService.GetMessage("InvalidCommercialRegistrationNumber"));

            RuleFor(x => x.TaxRegistrationNumber)
                .Matches(@"^\d{15}$") 
                .When(x => !string.IsNullOrEmpty(x.TaxRegistrationNumber))
                .WithMessage(messageService.GetMessage("InvalidTaxRegistrationNumber"));

            RuleFor(x => x.TaxCardNumber)
                .Matches(@"^\d{14}$") 
                .When(x => !string.IsNullOrEmpty(x.TaxCardNumber))
                .WithMessage(messageService.GetMessage("InvalidTaxCardNumber"));

            RuleFor(x => x.LicenseNumber)
                .Matches(@"^\d{6,12}$") 
                .When(x => !string.IsNullOrEmpty(x.LicenseNumber))
                .WithMessage(messageService.GetMessage("InvalidLicenseNumber"));

            RuleFor(x => x.EstablishmentDate)
                .LessThanOrEqualTo(DateTime.Today)
                .When(x => x.EstablishmentDate.HasValue)
                .WithMessage(messageService.GetMessage("InvalidEstablishmentDate"));

        }
    }
}
