using SS.Core.Database;
using SS.Core.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SS.Core
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new SSDbContext();

            var studentService = new StudentService(context);
            //await studentService.SeedStudents();

            var activityService = new ActivityService(context);
            //await activityService.SeedActivity();

            System.Console.WriteLine(studentService.GetCorrelationAnalysis());
            studentService
                .GetFrequency()
                .ToList()
                .ForEach(x => System.Console.WriteLine($"{x.Result} {x.RelativeFrequency} {x.AbsoluteFrequency}"));

        }
    }
}
