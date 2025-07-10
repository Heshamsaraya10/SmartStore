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
    public class ItemRequestValidator : AbstractValidator<ItemRequestDto>
    {
        private readonly IMessageService _messageService;

        public ItemRequestValidator(IMessageService messageService)
        {
            _messageService = messageService;

            RuleFor(x => x.NameArabic).ArabicName(_messageService);
            RuleFor(x => x.NameEnglish).EnglishName(_messageService);
            RuleFor(x => x.SalePrice).GreaterThan(0).WithMessage(_messageService.GetMessage("RequiredSalePrice"));
            RuleFor(x => x.PurchasePrice).GreaterThan(0).WithMessage(_messageService.GetMessage("RequiredPurchasePrice"));
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage(_messageService.GetMessage("RequiredCategory"));
            RuleFor(x => x.UnitId).GreaterThan(0).WithMessage(_messageService.GetMessage("RequiredUnit"));
            RuleFor(x => x.TypeId).GreaterThan(0).WithMessage(_messageService.GetMessage("RequiredType"));
        }


    }
}
