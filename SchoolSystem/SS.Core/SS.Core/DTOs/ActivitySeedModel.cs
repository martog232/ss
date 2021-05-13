namespace SS.Core.DTOs
{
    public class ActivitySeedModel
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public string EventContext { get; set; }
        public string Component { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }

        //public int StudentId
        //{
        //    get
        //    {
        //        return int.Parse(Description.Substring(18, 4));
        //    }
        //}
    }
}
