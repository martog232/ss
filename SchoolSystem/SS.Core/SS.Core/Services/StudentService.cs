using Ganss.Excel;
using Microsoft.EntityFrameworkCore;
using SS.Core.Database;
using SS.Core.DTOs;
using SS.Core.Models;
using SS.Core.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS.Core.Services
{
    public class StudentService : IStudentService
    {
        private const string filePath1 = @"C:\Users\Ali\Desktop\pts\InputData\Course A_StudentsResults_Year 1.xlsx";
        private const string filePath2 = @"C:\Users\Ali\Desktop\pts\InputData\Course A_StudentsResults_Year 2.xlsx";

        private const string COMPONENT = "File submissions";
        private const string EVENT_CONTEXT = "Assignment: Качване на курсови задачи и проекти";

        private readonly SSDbContext _context;
        private readonly ExcelMapper[] _excelMapper;

        public StudentService(SSDbContext context)
        {
            _context = context;
            _excelMapper = new ExcelMapper[]
            {
                new ExcelMapper(filePath1),
                new ExcelMapper(filePath2)
            };
        }

        public Student GetStudent(int id)
        {
            return _context.Students.Find(id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Students.ToList();
        }

        public IEnumerable<Student> GetStudentsByComponentAndEventContext(string component, string eventContext)
        {
            var studentIds = _context.Activities
                .Where(l => l.Component == component && l.EventContext == eventContext)
                .Select(l => l.StudentId)
                .Distinct()
                .ToListAsync()
                .Result;

            var students = _context.Students.ToListAsync().Result;
            var resultList = new List<Student>();

            foreach (var st in students)
            {
                foreach (var id in studentIds)
                {
                    if (st.Id == id) resultList.Add(st);
                }
            }

            return resultList;
        }

        public IEnumerable<FrequencyOutputModel> GetFrequency()
        {
            List<FrequencyOutputModel> abosoluteAndRelativeFrequency = new List<FrequencyOutputModel>();

            for (int i = 2; i <= 6; i++)
            {
                List<Student> studentsWithConcreteResults = GetStudentsByComponentAndEventContext(COMPONENT, EVENT_CONTEXT)
                    .Where(s => s.Result == (double)i)
                    .ToList();

                int absoluteFrequency = studentsWithConcreteResults.Count();
                double relativeFrequnecy = studentsWithConcreteResults.Count() / _context.Students.Count();
                abosoluteAndRelativeFrequency.Add(new FrequencyOutputModel(i, absoluteFrequency, relativeFrequnecy));
            }

            return abosoluteAndRelativeFrequency;
        }

        public double GetCorrelationAnalysis()
        {
            var absoluteAndRelativeFrequency = GetFrequency();
            var students = GetStudentsByComponentAndEventContext(COMPONENT, EVENT_CONTEXT);
            var result = ComputeCoeff(absoluteAndRelativeFrequency.Select(s => (double)s.Result), students.Select(s => s.Result));

            return result;
        }

        public CentralTendentionModel GetCentralTendention()
        {

            //Get Average
            double average = _context.Students.
                Select(s => s.Result).
                Average();

            //Get Mode
            var groupedStudents = _context.Students.GroupBy(x => x);
            var maxCount = groupedStudents.Max(g => g.Count());
            var mostCommons = groupedStudents.Where(x => x.Count() == maxCount).Select(x => x.Key).ToArray();
            double mode = mostCommons[0].Result;

            //Get Median


            return null;
        }

        public async Task SeedStudents()
        {
            var studentsYearOne = _excelMapper[0].Fetch<StudentSeedModel>();
            var studentsYearTwo = _excelMapper[1].Fetch<StudentSeedModel>();

            foreach (var dto in studentsYearOne)
            {
                await _context.Students.AddAsync(new Student(dto.Id, dto.Result));
            }

            foreach (var dto in studentsYearTwo)
            {
                await _context.Students.AddAsync(new Student(dto.Id, dto.Result));
            }

            await _context.SaveChangesAsync();
        }

        private double ComputeCoeff(IEnumerable<double> values1, IEnumerable<double> values2)
        {
            var avg1 = values1.Average();
            var avg2 = values2.Average();

            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sumSqr1 = values1.Sum(x => Math.Pow((x - avg1), 2.0));
            var sumSqr2 = values2.Sum(y => Math.Pow((y - avg2), 2.0));

            var result = sum1 / Math.Sqrt(sumSqr1 * sumSqr2);

            return result;
        }
    }
}
