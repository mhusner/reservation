using FluentValidation;
using ReservationsApi.Services;
namespace ReservationsApi.Api;

public static class ApiValidationContextExtensions
{
    public static void Validate<T>(this ApiValidationContext validationContext, IEnumerable<IValidator<T>> validators,
        T payload, bool ensureValid = true)
    {
        if (payload is null)
        {
            validationContext.AddFailure(new (default, "Empty body is not allowed!"));
        }
        else
        {
            foreach (var validator in validators)
            {
                var val = validator.Validate(payload);
                validationContext.AddFailures(val.Errors);
            }
        }

        if (ensureValid)
        {
            validationContext.EnsureContextIsValid();
        }
    }
}