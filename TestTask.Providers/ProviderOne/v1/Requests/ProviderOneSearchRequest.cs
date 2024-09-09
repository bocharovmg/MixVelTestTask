using System.ComponentModel.DataAnnotations;


namespace TestTask.Providers.ProviderOne.v1.Requests
{
    public class ProviderOneSearchRequest
    {
        // Mandatory
        // Start point of route, e.g. Moscow 
        [Required(AllowEmptyStrings = false)]
        public string From { get; set; } = string.Empty;

        // Mandatory
        // End point of route, e.g. Sochi
        [Required(AllowEmptyStrings = false)]
        public string To { get; set; } = string.Empty;

        // Mandatory
        // Start date of route
        public DateTime DateFrom { get; set; }

        // Optional
        // End date of route
        public DateTime? DateTo { get; set; }

        // Optional
        // Maximum price of route
        public decimal? MaxPrice { get; set; }
    }
}
