using MediatR;
using Microsoft.EntityFrameworkCore;
using ReservationsApi.Contracts;
using ReservationsApi.Data;
using ReservationsApi.Exceptions;

namespace ReservationsApi.Api.GetRegistration;

public class GetRegistrationQueryHandler : IRequestHandler<GetRegistrationQuery, Registration>
{
    private readonly AppDbContext _db;

    public GetRegistrationQueryHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Registration> Handle(GetRegistrationQuery request, CancellationToken cancellationToken)
    {
        var result = await _db.Reservations.AsNoTracking().FirstOrDefaultAsync(x => x.Apid == request.Id, cancellationToken: cancellationToken);
        if (result == null)
        {
            throw new ApiNotFoundException();
        }

        return new Registration()
        {
            Id = result.Apid,
            Date = result.Date,
            Name = result.Name
        };
    }
}