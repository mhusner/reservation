using System;
using FluentValidation;

namespace ReservationsApi.Contracts.Validators
{
    public class RegistrationCreateValidator : AbstractValidator<RegistrationCreate>
    {
        public RegistrationCreateValidator()
        {
            RuleFor(prop => prop.Name).NotEmpty().WithMessage("Name is required!");
            RuleFor(prop => prop.Name).MaximumLength(30).WithMessage("Max length of name is 30 characters!");
            RuleFor(prop => prop.Date).NotEmpty().WithMessage("Date is required!");
        }
    }
}