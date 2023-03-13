using MediatR;
using ReservationsApi.Contracts;

namespace ReservationsApi.Api.CreateRegistration;

public class CreateRegistrationCommand : RegistrationCreate, IRequest<string>
{
    public CreateRegistrationCommand(RegistrationCreate payload)
    {
        Payload = payload;
    }

    public RegistrationCreate Payload { get; }
}