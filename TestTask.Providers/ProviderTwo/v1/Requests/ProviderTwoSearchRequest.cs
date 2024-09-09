using System.ComponentModel.DataAnnotations;


namespace TestTask.Providers.ProviderTwo.v1.Requests
{
    public class ProviderTwoSearchRequest
    {
        // Mandatory
        // Start point of route, e.g. Moscow 
        [Required(AllowEmptyStrings = false)]
        public string Departure { get; set; } = string.Empty;

        // Mandatory
        // End point of route, e.g. Sochi
        [Required(AllowEmptyStrings = false)]
        public string Arrival { get; set; } = string.Empty;

        // Mandatory
        // Start date of route
        public DateTime DepartureDate { get; set; }

        // Optional
        // Minimum value of timelimit for route
        public DateTime? MinTimeLimit { get; set; }
    }
}
