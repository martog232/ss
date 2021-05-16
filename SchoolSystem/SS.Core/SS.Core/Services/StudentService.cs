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

        public IEnumerable<Student> GetStudentsByComponentAndEventContext()
        {
            var studentIds = _context.Activities
                .Where(l => l.Component == COMPONENT && l.EventContext == EVENT_CONTEXT)
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
                List<Student> studentsWithConcreteResults = GetStudentsByComponentAndEventContext()
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
            var students = GetStudentsByComponentAndEventContext();
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

        public DispersionOutputModel GetDispersionOutput()
        {
            //Deviation
            IEnumerable<Student> students = GetStudentsByComponentAndEventContext();

            int studentsCount = students.Count();

            if (studentsCount == 0)
            {
                //throw new NoStudentsForStandardDeviationException("Cannot calculate standard deviation");
            }

            double average = students.
                Select(s => s.Result).
                Average();

            double sum = 0.0;

            foreach (Student s in students)
            {
                double step2 = s.Result - average;
                double result = Math.Pow(step2, 2);
                sum += result;
            }
            sum /= studentsCount;

            double deviation = Math.Sqrt(sum);

            //Scope

            double maxResult = students.Select(s => s.Result).Max();
            double minResult = students.Select(s => s.Result).Min();
            double scope = maxResult - minResult;

            //Dispersion

            double sumOfStudentsResults = 0;

            foreach (Student student in students)
            {
                sumOfStudentsResults += student.Result;
            }

            double dispersion = 0;


            foreach (Student student in students)
            {
                dispersion += (student.Result - sumOfStudentsResults / studentsCount * (student.Result - (sumOfStudentsResults / studentsCount))) * sumOfStudentsResults / sumOfStudentsResults;
            }

            dispersion = Math.Sqrt(dispersion);

            return new DispersionOutputModel(scope, deviation, dispersion);
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
