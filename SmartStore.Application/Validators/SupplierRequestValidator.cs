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
    public class SupplierRequestValidator : AbstractValidator<SupplierRequestDto>
    {
        private readonly IMessageService _messageService;

        public SupplierRequestValidator(IMessageService messageService)
        {
            _messageService = messageService;


            RuleFor(x => x.NameArabic).ArabicName(_messageService);
            RuleFor(x => x.NameEnglish).EnglishName(_messageService);
            RuleFor(x => x.Phone).NotEmpty().WithMessage(_messageService.GetMessage("RequiredPhone"));
        }
    }
}
