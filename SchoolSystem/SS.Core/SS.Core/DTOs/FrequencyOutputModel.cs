namespace SS.Core.DTOs
{
    public class FrequencyOutputModel
    {
        public FrequencyOutputModel(int result, int absoluteFrequency, double relativeFrequency)
        {
            Result = result;
            AbsoluteFrequency = absoluteFrequency;
            RelativeFrequency = relativeFrequency;
        }

        public int Result { get; set; }
        public int AbsoluteFrequency { get; set; }
        public double RelativeFrequency { get; set; }
    }
}
