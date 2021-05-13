using SS.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SS.Core.Services.Contracts
{
    interface IStudentService
    {
        public Student GetStudent(int id);
        public IEnumerable<Student> GetStudentsByComponentAndEventContext(string component, string eventContext);
        public IEnumerable<Student> GetStudents();
        public Task SeedStudents();
    }
}
