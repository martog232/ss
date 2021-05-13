using System.ComponentModel.DataAnnotations;

namespace SS.Core.Models
{
    public class Activity
    {
        public Activity() { }
        public Activity(string dateAsString, string evContext, string component, string evName, string description)
        {
            Time = dateAsString;
            EventContext = evContext;
            Component = component;
            EventName = evName;
            Description = description;
            StudentId = int.Parse(Description.Substring(18, 4));
        }

        [Key]
        public int Id { get; set; }
        public string Time { get; set; }
        public string EventContext { get; set; }
        public string Component { get; set; }
        public string EventName { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
