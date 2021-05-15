namespace SS.Core.DTOs
{
    public class DispersionOutputModel
    {
        public DispersionOutputModel(double scope, double deviation, double dispersion)
        {
            Scope = scope;
            Deviation = deviation;
            Dispersion = dispersion;
        }

        public double Scope { get; set; }
        public double Deviation { get; set; }
        public double Dispersion { get; set; }
    }
}
