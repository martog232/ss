using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Core.Models
{
    public class Student
    {
        public Student() { }
        // public Student(int id, double result) 
        // { 
        //     Id = id; 
        //     Result = result; 
        // }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int Id { get; set; }
        public double Result { get; set; }
        public ICollection<StudentsCourses> StudentsCourses { get; set; }
        public ICollection<Activity> Activities { get; set; }

    }
}
