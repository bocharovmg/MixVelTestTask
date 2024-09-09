using System.ComponentModel.DataAnnotations;


namespace TestTask.Domain.Contracts.v1.Dtos
{
    public class Route
    {
        // Mandatory
        // Identifier of the whole route
        [Required]
        public Guid Id { get; set; }

        // Mandatory
        // Start point of route
        [Required(AllowEmptyStrings = false)]
        public string Origin { get; set; } = string.Empty;

        // Mandatory
        // End point of route
        [Required(AllowEmptyStrings = false)]
        public string Destination { get; set; } = string.Empty;

        // Mandatory
        // Start date of route
        [Required]
        public DateTime OriginDateTime { get; set; }

        // Mandatory
        // End date of route
        [Required]
        public DateTime DestinationDateTime { get; set; }

        // Mandatory
        // Price of route
        [Required]
        public decimal Price { get; set; }

        // Mandatory
        // Timelimit. After it expires, route became not actual
        [Required]
        public DateTime TimeLimit { get; set; }

        public override int GetHashCode()
        {
            int originHashCode = Origin.GetHashCode();

            int destinationHashCode = Destination.GetHashCode();

            int originDateTimeHashCode = OriginDateTime.GetHashCode();

            int dDestinationDateTimeHashCode = DestinationDateTime.GetHashCode();

            int priceHashCode = Price.GetHashCode();

            return originHashCode
                ^ destinationHashCode
                ^ originDateTimeHashCode
                ^ dDestinationDateTimeHashCode
                ^ priceHashCode;
        }
    }
}
