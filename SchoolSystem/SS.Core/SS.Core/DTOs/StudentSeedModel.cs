using SS.Core.Models;

namespace SS.Core.DTOs
{
    public class StudentSeedModel
    {

        public StudentSeedModel(Student student)
        {
            Id = student.Id;
            Result = student.Result;
        }
        public int Id { get; set; }
        public double Result { get; set; }
    }
}
