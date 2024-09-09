namespace TestTask.Providers.ProviderTwo.v1.Dtos
{
    public class ProviderTwoPoint
    {
        // Mandatory
        // Name of point, e.g. Moscow\Sochi
        public string Point { get; set; } = string.Empty;

        // Mandatory
        // Date for point in Route, e.g. Point = Moscow, Date = 2023-01-01 15-00-00
        public DateTime Date { get; set; }
    }
}
