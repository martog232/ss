namespace SS.Core.DTOs
{
    public class CentralTendentionModel
    {
        public CentralTendentionModel(double mode, double median, double average)
        {
            Mode = mode;
            Median = median;
            Average = average;
        }

        public double Mode { get; set; }
        public double Median { get; set; }
        public double Average { get; set; }
    }
}
