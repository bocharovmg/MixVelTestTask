using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using TestTask.Domain.Contracts.v1.Dtos;
using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Domain.Contracts.v1.Responses;
using TestTask.Domain.Services.v1;


namespace TestTask.Application.Services.v1
{
    /// <summary>
    /// Represents a caching and searching service
    /// </summary>
    public class SearchCacheService : ISearchCacheService
    {
        private readonly ILogger<SearchCacheService> _logger;

        private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

        private readonly MemoryCache _cache;

        public SearchCacheService(ILogger<SearchCacheService> logger)
        {
            _logger = logger;

            _cache = new MemoryCache(new MemoryCacheOptions()
            {
                SizeLimit = 1024
            });
        }

        public async Task<IEnumerable<object>> AddAsync(IEnumerable<Route> routes, CancellationToken cancellationToken)
        {
            var result = new List<object>();

            foreach (var route in routes)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var key = route.GetHashCode();

                var semaphore = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

                await semaphore.WaitAsync(cancellationToken);

                try
                {
                    if (_cache.TryGetValue(key, out var _))
                    {
                        _cache.Remove(key);
                    }

                    var timeLimit = route.TimeLimit.Subtract(DateTime.MinValue);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSize(1)
                        .SetPriority(CacheItemPriority.High)
                        .SetAbsoluteExpiration(timeLimit)
                        .RegisterPostEvictionCallback(PostEviction);

                    _cache.Set(key, route, cacheEntryOptions);

                    result.Add(key);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add the route to the cache");
                }
                finally
                {
                    semaphore.Release();
                }
            }

            return result;
        }

        public async Task<int> RemoveAsync(IEnumerable<object> keys, CancellationToken cancellationToken)
        {
            var result = 0;

            foreach (var key in keys)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!_locks.TryGetValue(key, out var semaphore))
                {
                    continue;
                }

                await semaphore.WaitAsync(cancellationToken);

                try
                {
                    if (_cache.TryGetValue(key, out var _))
                    {
                        _cache.Remove(key);
                    }

                    _locks.Remove(key, out var _);
                    
                    result++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to remove the route from the cache");
                }
                finally
                {
                    semaphore.Release();
                }
            }

            return result;
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            object searchResultLockObject = new object();

            var routes = new List<Route>();

            decimal minPrice = 0;

            decimal maxPrice = 0;

            int minMinutesRoute = 0;

            int maxMinutesRoute = 0;

            await Parallel.ForEachAsync(
                _locks,
                cancellationToken,
                async (lockPair, cancellationToken) =>
                {
                    var key = lockPair.Key;

                    var semaphore = lockPair.Value;

                    await semaphore.WaitAsync(cancellationToken);

                    try
                    {
                        _cache.TryGetValue(key, out Route? route);

                        if (route == null)
                        {
                            return;
                        }

                        if (request.Filters != null)
                        {
                            var destinationDateTimeDiff = request.Filters.DestinationDateTime.HasValue ?
                                route.DestinationDateTime.Subtract(request.Filters.DestinationDateTime.GetValueOrDefault()).TotalMinutes : 0;

                            if (destinationDateTimeDiff != 0)
                            {
                                return;
                            }
                            
                            if (
                                request.Filters.MaxPrice.HasValue
                                && route.Price > request.Filters.MaxPrice
                            )
                            {
                                return;
                            }
                            
                            if (
                                request.Filters.MinTimeLimit.HasValue
                                && route.TimeLimit < request.Filters.MinTimeLimit
                            )
                            {
                                return;
                            }
                        }

                        if (route.Destination != request.Destination)
                        {
                            return;
                        }

                        if (route.Origin != request.Origin)
                        {
                            return;
                        }

                        var originDateTimeDiff = route.OriginDateTime.Subtract(request.OriginDateTime).TotalMinutes;

                        if (originDateTimeDiff != 0)
                        {
                            return;
                        }

                        var minutesRoute = (int)route.DestinationDateTime.Subtract(route.OriginDateTime).TotalMinutes;

                        lock (searchResultLockObject)
                        {
                            routes.Add(route);

                            minPrice = minPrice == 0 || minPrice > route.Price ? route.Price : minPrice;

                            maxPrice = maxPrice == 0 || maxPrice < route.Price ? route.Price : maxPrice;

                            minMinutesRoute = minMinutesRoute == 0 || minMinutesRoute > minutesRoute ? minutesRoute : minMinutesRoute;

                            maxMinutesRoute = maxMinutesRoute == 0 || maxMinutesRoute < minutesRoute ? minutesRoute : maxMinutesRoute;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to search the route in the cache");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }
            );

            return new SearchResponse
            {
                Routes = routes.ToArray(),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinMinutesRoute = minMinutesRoute,
                MaxMinutesRoute = maxMinutesRoute
            };
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            return true;
        }

        private void PostEviction(object key, object? value, EvictionReason reason, object? state)
        {
            if (
                reason == EvictionReason.TokenExpired
                || reason == EvictionReason.Expired
                || reason == EvictionReason.Removed
                || reason == EvictionReason.Capacity
            )
            {
                _locks.Remove(key, out var _);
            }
        }
    }
}
