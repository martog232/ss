using SS.Core.Models;
using System.Collections.Generic;

namespace SS.Core.Services.Contracts
{
    interface ICourseService
    {
        public Course GetCourse(int id);
        public IEnumerable<Course> GetCourses();
    }
}
