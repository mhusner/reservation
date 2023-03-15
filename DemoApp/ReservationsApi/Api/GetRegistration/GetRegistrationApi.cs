using MediatR;
using ReservationsApi.Contracts;

namespace ReservationsApi.Api.GetRegistration;

public static class GetRegistrationApi
{
    public static WebApplication GetRegistration(this WebApplication app)
    {
        app.MapMethods("registrations/{id}", new[] {"GET", "HEAD"}, async (string id, IMediator mediatr, CancellationToken ctx) =>
            {
                var query = new GetRegistrationQuery(id);
                Registration data = await mediatr.Send(query, ctx);

                return Results.Ok(data);
            })
            .With("Registrations", nameof(GetRegistration), CorsPolicies.Default, 400, 404, 500)
            .Produces<Registration>(StatusCodes.Status200OK, "application/json");

        return app;
    }
}