using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReservationsApi.Contracts;
using ReservationsApi.Data;

namespace ReservationsApi.Api.CreateRegistration;

public class RegistrationCreateApiValidator : AbstractValidator<RegistrationCreate>
{
    private readonly AppDbContext _appDbContext;

    public RegistrationCreateApiValidator(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

        RuleFor(prop => prop.Date)
            .Must(x => HasFreePlaces(5, x.Value.Date))
            .WithMessage("No free spaces for this date!");
    }

    private bool HasFreePlaces(int max, DateTime date)
    {
        var all = _appDbContext.Reservations.AsNoTracking().Count(x => x.Date.Date == date);
        if (all >= max)
        {
            return false;
        }

        return true;
    }
}