using Ganss.Excel;
using Microsoft.EntityFrameworkCore;
using SS.Core.Database;
using SS.Core.DTOs;
using SS.Core.Models;
using SS.Core.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS.Core.Services
{
    public class StudentService : IStudentService
    {
        private const string filePath1 = @"C:\Users\Ali\Desktop\pts\InputData\Course A_StudentsResults_Year 1.xlsx";
        private const string filePath2 = @"C:\Users\Ali\Desktop\pts\InputData\Course A_StudentsResults_Year 2.xlsx";

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
                    //if (st.Id == id) resultList.Add(st);
                }
            }

            return resultList;
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
    }
}
