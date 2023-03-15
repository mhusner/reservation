using MediatR;
using ReservationsApi.Contracts;

namespace ReservationsApi.Api.GetRegistration;

public class GetRegistrationQuery : IRequest<Registration>, ICacheable
{
    public GetRegistrationQuery(string id)
    {
        Id = id;
    }
    
    public string Id { get; }
    public string CacheKey => Id;
    public TimeSpan CacheDuration => TimeSpan.FromSeconds(30);
}