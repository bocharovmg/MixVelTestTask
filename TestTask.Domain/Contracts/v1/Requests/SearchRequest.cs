using System.ComponentModel.DataAnnotations;
using TestTask.Domain.Contracts.v1.Dtos;


namespace TestTask.Domain.Contracts.v1.Requests
{
    public class SearchRequest
    {
        // Mandatory
        // Start point of route, e.g. Moscow 
        [Required(AllowEmptyStrings = false)]
        public string Origin { get; set; } = string.Empty;

        // Mandatory
        // End point of route, e.g. Sochi
        [Required(AllowEmptyStrings = false)]
        public string Destination { get; set; } = string.Empty;

        // Mandatory
        // Start date of route
        [Required]
        public DateTime OriginDateTime { get; set; }

        // Optional
        public SearchFilters? Filters { get; set; }
    }
}
