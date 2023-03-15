namespace ReservationsApi.Api;

public interface ICacheable
{
    public string CacheKey { get; }
    public TimeSpan CacheDuration { get; }
}