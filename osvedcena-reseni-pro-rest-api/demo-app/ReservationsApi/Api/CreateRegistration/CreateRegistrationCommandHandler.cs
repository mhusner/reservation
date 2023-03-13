using FluentValidation;
using MediatR;
using ReservationsApi.Contracts;
using ReservationsApi.Data;
using ReservationsApi.Services;

namespace ReservationsApi.Api.CreateRegistration;

public class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand, string>
{
    private readonly AppDbContext _appDbContext;
    private readonly IEnumerable<IValidator<RegistrationCreate>> _validators;
    private readonly ApiValidationContext _validationContext;

    public CreateRegistrationCommandHandler(AppDbContext appDbContext, IEnumerable<IValidator<RegistrationCreate>> validators, ApiValidationContext validationContext)
    {
        _appDbContext = appDbContext;
        _validators = validators;
        _validationContext = validationContext;
    }

    public async Task<string> Handle(CreateRegistrationCommand request, CancellationToken cancellationToken)
    {
        _validationContext.Validate(_validators, request.Payload, ensureValid:false);

        Reservation reservation = new()
        {
            Apid = RandomIdGenerator.Create(),
            Date = request.Payload.Date!.Value,
            Name = request.Payload.Name
        };

        await _appDbContext.Reservations.AddAsync(reservation, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return reservation.Apid;
    }
}