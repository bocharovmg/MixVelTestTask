using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Domain.Contracts.v1.Responses;


namespace TestTask.Domain.Services.v1
{
    /// <summary>
    /// Defines a mechanism for searching for routes and checking the availability of the service
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Search for routes
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns a <see cref="SearchResponse"/></returns>
        Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Checking the availability of the service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns true if the service is available. Otherwise - false</returns>
        Task<bool> IsAvailableAsync(CancellationToken cancellationToken);
    }
}
