using TestTask.Domain.Contracts.v1.Dtos;


namespace TestTask.Domain.Services.v1
{
    /// <summary>
    /// Defines the route caching mechanism
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Add routes to the cache
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns keys for added routes</returns>
        Task<IEnumerable<object>> AddAsync(IEnumerable<Route> routes, CancellationToken cancellationToken);

        /// <summary>
        /// Deleting routes by keys from the cache
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> RemoveAsync(IEnumerable<object> keys, CancellationToken cancellationToken);
    }
}
