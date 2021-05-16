using SS.Core.DTOs;
using SS.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SS.Core.Services.Contracts
{
    public interface IStudentService
    {
        public Student GetStudent(int id);
        public IEnumerable<Student> GetStudentsByComponentAndEventContext();
        public IEnumerable<Student> GetStudents();
        public IEnumerable<FrequencyOutputModel> GetFrequency();
        public double GetCorrelationAnalysis();
        public CentralTendentionModel GetCentralTendention();
        public DispersionOutputModel GetDispersionOutput();
        public Task SeedStudents();
    }
}
