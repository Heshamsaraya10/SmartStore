using FluentValidation;
using SmartStore.Application.Services.ApplicationServices.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

public static class NameValidator
{
    private static readonly List<string> ForbiddenArabicNames = new List<string> { "اسم", "تجريبي", "غير معروف", "فارغ" }
        .Select(n => n.ToLowerInvariant()).ToList();

    private static readonly List<string> ForbiddenEnglishNames = new List<string> { "string", "test", "unknown", "empty" }
        .Select(n => n.ToLowerInvariant()).ToList();

    public static IRuleBuilderOptions<T, string> ArabicName<T>(this IRuleBuilder<T, string> ruleBuilder, IMessageService messageService)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(messageService.GetMessage("RequiredNameArabic"))
            .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage(messageService.GetMessage("ArabicOnly"))
            .Must(name => !ForbiddenArabicNames.Contains(name?.Trim().ToLowerInvariant()))
            .WithMessage(messageService.GetMessage("ForbiddenName"));
    }

    public static IRuleBuilderOptions<T, string> EnglishName<T>(this IRuleBuilder<T, string> ruleBuilder, IMessageService messageService)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(messageService.GetMessage("RequiredNameEnglish"))
            .Matches(@"^[a-zA-Z\s]+$").WithMessage(messageService.GetMessage("EnglishOnly"))
            .Must(name => !ForbiddenEnglishNames.Contains(name?.Trim().ToLowerInvariant()))
            .WithMessage(messageService.GetMessage("ForbiddenName"));
    }
}
