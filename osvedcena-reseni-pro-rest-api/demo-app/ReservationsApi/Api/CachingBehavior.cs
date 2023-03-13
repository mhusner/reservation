using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ReservationsApi.Api;

public class CachingBehavior<TReq, TResp> : IPipelineBehavior<TReq, TResp>
where TReq : ICacheable, IRequest<TResp>
{
    private readonly IMemoryCache _memoryCache;

    public CachingBehavior(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<TResp> Handle(TReq request, CancellationToken cancellationToken, RequestHandlerDelegate<TResp> next)
    {
        if (_memoryCache.TryGetValue(request.CacheKey, out TResp res))
        {
            return res;
        }

        var freshData = await next();
        _memoryCache.Set(request.CacheKey, freshData, request.CacheDuration);

        return freshData;
    }
}