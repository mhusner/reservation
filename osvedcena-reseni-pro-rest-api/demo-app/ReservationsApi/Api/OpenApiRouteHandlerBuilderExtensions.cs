namespace ReservationsApi.Api;

public static class OpenApiRouteHandlerBuilderExtensions
{
    /// <summary>
    /// Ukázka extension metody pro minimal API
    /// </summary>
    public static RouteHandlerBuilder With(this RouteHandlerBuilder route, string tag, string name, string corsPolicy, params int[] errorCodes)
    {
        route
            .WithTags(tag)
            .WithName(name)
            .WithDisplayName(name)
            .RequireCors(corsPolicy);

        foreach (var code in errorCodes)
        {
            if (code == StatusCodes.Status400BadRequest)
            {
                route.ProducesValidationProblem(StatusCodes.Status400BadRequest, "application/json");
            }
            else
            {
                route.ProducesProblem(code, "application/json");
            }
        }

        return route;
    }
}