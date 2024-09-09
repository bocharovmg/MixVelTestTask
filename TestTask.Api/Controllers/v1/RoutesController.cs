using Microsoft.AspNetCore.Mvc;
using TestTask.Domain.Contracts.v1.Requests;
using TestTask.Domain.Services.v1;


namespace TestTask.Api.Controllers.v1
{
    /// <summary>
    /// Represents a routes search API. API version 1
    /// </summary>
    [ApiController]
    [Route("routes/v1")]
    public class RoutesController : ControllerBase
    {
        private readonly ILogger<RoutesController> _logger;

        private readonly ISearchService _searchService;

        public RoutesController(
            ILogger<RoutesController> logger,
            ISearchService searchService
        )
        {
            _logger = logger;

            _searchService = searchService;
        }

        /// <summary>
        /// Search for routes
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> SearchAsync([FromBody] SearchRequest searchRequest, CancellationToken cancellationToken)
        {
            var response = await _searchService.SearchAsync(searchRequest, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Checking the availability of the service
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("ping")]
        public async Task<IActionResult> PingAsync(CancellationToken cancellationToken)
        {
            var result = await _searchService.IsAvailableAsync(cancellationToken);

            if (result)
            {
                return Ok();
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
