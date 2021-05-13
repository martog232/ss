using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SS.Core.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public ICollection<StudentsCourses> StudentsCourses { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}
