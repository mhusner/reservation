using MediatR;
using ReservationsApi.Api.GetRegistration;
using ReservationsApi.Contracts;

namespace ReservationsApi.Api.CreateRegistration;

public static class CreateRegistrationApi
{
    public static WebApplication CreateRegistration(this WebApplication app)
    {
        app.MapPost("registrations", async (RegistrationCreate model, IMediator mediatr, CancellationToken ctx) =>
            {
                var command = new CreateRegistrationCommand(model);
                string id = await mediatr.Send(command, ctx);

                var query = new GetRegistrationQuery(id);
                Registration data = await mediatr.Send(query, ctx);

                return Results.CreatedAtRoute(nameof(GetRegistrationApi.GetRegistration), new {id}, data);
            })
            .With("Registrations", nameof(CreateRegistration), CorsPolicies.Default, 400, 500)
            .Accepts<RegistrationCreate>("application/json")
            .Produces<Registration>(StatusCodes.Status200OK, "application/json");

        return app;
    }
}